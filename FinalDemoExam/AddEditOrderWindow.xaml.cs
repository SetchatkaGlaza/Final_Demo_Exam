using FinalDemoExam.Entity;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FinalDemoExam
{
    public partial class AddEditOrderWindow : Window
    {
        private readonly Orders order;
        private readonly bool isEdit;
        private readonly ObservableCollection<OrderItems> items = new ObservableCollection<OrderItems>();

        public AddEditOrderWindow(Orders selectedOrder = null)
        {
            InitializeComponent();
            order = selectedOrder ?? new Orders();
            isEdit = selectedOrder != null;
            try
            {
                cmbStatus.ItemsSource = App.DB.OrderStatuses.ToList();
                cmbPickupPoint.ItemsSource = App.DB.PickupPoints.ToList();
                cmbUser.ItemsSource = App.DB.Users.ToList();
                productColumn.ItemsSource = App.DB.Products.ToList();
            }
            catch (Exception ex)
            {
                Warn("Не удалось загрузить справочники заказа: " + ex.Message);
                Close();
                return;
            }

            if (isEdit)
            {
                FillForm();
            }
            else
            {
                dpOrderDate.SelectedDate = DateTime.Today;
                dpDeliveryDate.SelectedDate = DateTime.Today.AddDays(3);
            }

            dgOrderItems.ItemsSource = items;
        }

        private void FillForm()
        {
            txtId.Text = order.id.ToString();
            cmbStatus.SelectedValue = order.status_id;
            cmbPickupPoint.SelectedValue = order.pickup_point_id;
            cmbUser.SelectedValue = order.user_id;
            dpOrderDate.SelectedDate = order.order_date;
            dpDeliveryDate.SelectedDate = order.delivery_date;
            btnDelete.Visibility = Visibility.Visible;
            foreach (var item in order.OrderItems) items.Add(new OrderItems { product_article = item.product_article, quantity = item.quantity });
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ReadForm()) return;
            if (!isEdit)
            {
                order.id = GetNextOrderId();
                order.pickup_code = GetUniquePickupCode();
                App.DB.Orders.Add(order);
            }

            order.status_id = (int)cmbStatus.SelectedValue;
            order.pickup_point_id = (int)cmbPickupPoint.SelectedValue;
            order.user_id = (int)cmbUser.SelectedValue;
            order.order_date = dpOrderDate.SelectedDate.Value;
            order.delivery_date = dpDeliveryDate.SelectedDate.Value;
            try
            {
                foreach (var oldItem in order.OrderItems.ToList()) App.DB.OrderItems.Remove(oldItem);
                foreach (var item in items) App.DB.OrderItems.Add(new OrderItems { order_id = order.id, product_article = item.product_article, quantity = item.quantity });
                App.DB.SaveChanges();
                DialogResult = true;
            }
            catch (Exception ex)
            {
                Warn("Не удалось сохранить заказ: " + ex.Message);
            }
        }

        private bool ReadForm()
        {
            if (cmbStatus.SelectedValue == null || cmbPickupPoint.SelectedValue == null || cmbUser.SelectedValue == null) return Warn("Выберите статус, пункт выдачи и клиента.");
            if (!dpOrderDate.SelectedDate.HasValue || !dpDeliveryDate.SelectedDate.HasValue) return Warn("Выберите дату заказа и дату выдачи.");
            if (dpDeliveryDate.SelectedDate.Value.Date < dpOrderDate.SelectedDate.Value.Date) return Warn("Дата выдачи не может быть раньше даты заказа.");
            if (items.Count == 0 || items.Any(i => string.IsNullOrWhiteSpace(i.product_article) || i.quantity <= 0)) return Warn("Добавьте товары заказа и укажите положительное количество.");
            if (items.GroupBy(i => i.product_article).Any(g => g.Count() > 1)) return Warn("Один товар нельзя добавлять в заказ несколько раз. Измените количество в существующей строке.");
            return true;
        }

        private int GetNextOrderId()
        {
            return App.DB.Orders.Any() ? App.DB.Orders.Max(o => o.id) + 1 : 1;
        }

        private int GetUniquePickupCode()
        {
            var random = new Random();
            int pickupCode;
            do
            {
                pickupCode = random.Next(100, 1000);
            }
            while (App.DB.Orders.Any(o => o.pickup_code == pickupCode));

            return pickupCode;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!isEdit) return;
            if (MessageBox.Show("Удалить выбранный заказ?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            try
            {
                foreach (var item in order.OrderItems.ToList()) App.DB.OrderItems.Remove(item);
                App.DB.Orders.Remove(order);
                App.DB.SaveChanges();
                DialogResult = true;
            }
            catch (Exception ex)
            {
                Warn("Не удалось удалить заказ: " + ex.Message);
            }
        }

        private bool Warn(string text)
        {
            MessageBox.Show(text, "Проверьте данные", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
