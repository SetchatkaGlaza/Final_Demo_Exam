using FinalDemoExam.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinalDemoExam
{
    public partial class MainWindow : Window
    {
        private List<Products> products;
        public MainWindow()
        {
            InitializeComponent();
            txtUserFullName.Text = App.UserFullName;
            Setup();
            Refresh();
            ApplyFilters();
        }

        public void Setup()
        {
            bool isAdmin = App.UserRole == 1;
            bool isManager = App.UserRole == 2;
            bool canEdit = isAdmin || isManager;

            btnAdd.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
            btnOrders.Visibility = canEdit ? Visibility.Visible : Visibility.Collapsed;
            filterPanel.Visibility = canEdit ? Visibility.Visible : Visibility.Collapsed;
        }

        public void Refresh()
        {
            try
            {
                products = App.DB.Products.Include("Categories")
                    .Include("Manufacturers")
                    .Include("Suppliers")
                    .Include("Units")
                    .ToList();
            }
            catch
            {
                products = new List<Products>();
                MessageBox.Show("Не удалось загрузить товары. Проверьте подключение к БД.");
            }
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (products == null) return;

            var result = products.AsEnumerable();

            string searchText = txtSearch?.Text?.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                result = result.Where(p =>
                    p.article?.ToLower().Contains(searchText) == true ||
                    p.name?.ToLower().Contains(searchText) == true ||
                    p.description?.ToLower().Contains(searchText) == true ||
                    p.Categories?.name?.ToLower().Contains(searchText) == true ||
                    p.Manufacturers?.name?.ToLower().Contains(searchText) == true ||
                    p.Suppliers?.name?.ToLower().Contains(searchText) == true
                );
            }

            string filter = (cmbFilter?.SelectedItem as ComboBoxItem)?.Content?.ToString();
            if (filter == "0-12,99%") result = result.Where(p => p.discount < 13);
            else if (filter == "13-16,99%") result = result.Where(p => p.discount >= 13 && p.discount < 17);
            else if (filter == "17% и более") result = result.Where(p => p.discount >= 17);

            string sort = (cmbSort?.SelectedItem as ComboBoxItem)?.Content?.ToString();
            switch (sort)
            {
                case "Цена по возрастанию":
                    result = result.OrderBy(p => p.price);
                    break;
                case "Цена по убыванию":
                    result = result.OrderByDescending(p => p.price);
                    break;
                case "Количество по возрастанию":
                    result = result.OrderBy(p => p.stock_quantity);
                    break;
                case "Количество по убыванию":
                    result = result.OrderByDescending(p => p.stock_quantity);
                    break;
                default:
                    break;
            }

            lvProducts.ItemsSource = result.ToList();
        }

        private void FilterChanged(object sender, RoutedEventArgs e) => ApplyFilters();

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            new AddEditWindow().ShowDialog();
            Refresh();
        }

        private void lvProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (App.UserRole != 1) return;
            if (lvProducts.SelectedItem is Products product)
            {
                new AddEditWindow(product).ShowDialog();
                Refresh();
            }
        }

        private void btnOrders_Click(object sender, RoutedEventArgs e) => new ListOrderWindow().ShowDialog();

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }
    }
}