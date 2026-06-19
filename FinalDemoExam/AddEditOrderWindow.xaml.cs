using FinalDemoExam.Entity;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FinalDemoExam
{
    public partial class AddEditOrderWindow : Window
    {
        private Orders order;
        private bool isEdit;
        private ObservableCollection<OrderItems> orderItems;

        public AddEditOrderWindow(Orders o = null)
        {
            InitializeComponent();
            isEdit = o != null;
            order = o ?? new Orders();

            cmbStatus.ItemsSource = App.DB.OrderStatuses.ToList();
            cmbPickupPoint.ItemsSource = App.DB.PickupPoints.ToList();
            cmbUser.ItemsSource = App.DB.Users.ToList();

            orderItems = new ObservableCollection<OrderItems>();

            if (isEdit)
            {
                txtId.Text = order.id.ToString();
                cmbStatus.SelectedValue = order.status_id;
                dpOrderDate.SelectedDate = order.order_date;
                dpDeliveryDate.SelectedDate = order.delivery_date;
                cmbPickupPoint.SelectedValue = order.pickup_point_id;
                cmbUser.SelectedValue = order.user_id;

                foreach (var item in order.OrderItems)
                {
                    orderItems.Add(new OrderItems { order_id = item.order_id, product_article = item.product_article, quantity = item.quantity });
                }
            }

            dgOrderItems.ItemsSource = orderItems;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!dpOrderDate.SelectedDate.HasValue || !dpDeliveryDate.SelectedDate.HasValue)
                {
                    MessageBox.Show("Выберите даты!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (orderItems.Count == 0)
                {
                    MessageBox.Show("Добавьте хотя бы один товар!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                order.status_id = (int)cmbStatus.SelectedValue;
                order.order_date = dpOrderDate.SelectedDate.Value;
                order.delivery_date = dpDeliveryDate.SelectedDate.Value;
                order.pickup_point_id = (int)cmbPickupPoint.SelectedValue;
                order.user_id = (int)cmbUser.SelectedValue;

                if (!isEdit)
                {
                    order.id = App.DB.Orders.Any() ? App.DB.Orders.Max(o => o.id) + 1 : 1;
                    order.pickup_code = new Random().Next(100, 999);
                    App.DB.Orders.Add(order);
                    App.DB.SaveChanges();

                    foreach (var item in orderItems)
                    {
                        item.order_id = order.id;
                        App.DB.OrderItems.Add(item);
                    }
                }

                App.DB.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}