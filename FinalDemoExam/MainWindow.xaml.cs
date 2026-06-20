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
        private readonly List<Products> emptyProducts = new List<Products>();
        private List<Products> products = new List<Products>();
        private bool isWindowReady;

        public MainWindow()
        {
            InitializeComponent();
            txtUserFullName.Text = App.UserFullName;

            bool isAdmin = App.UserRole == 1; // ID администратора
            bool isManager = App.UserRole == 2;
            bool canSearch = isAdmin || isManager;

            btnAdd.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
            btnOrders.Visibility = canSearch ? Visibility.Visible : Visibility.Collapsed;
            filtersPanel.Visibility = canSearch ? Visibility.Visible : Visibility.Collapsed;

            isWindowReady = true;
            Refresh();
        }

        public void Refresh()
        {
            try
            {
                products = App.DB.Products
                    .Include(p => p.Categories)
                    .Include(p => p.Manufacturers)
                    .Include(p => p.Suppliers)
                    .Include(p => p.Units)
                    .ToList();
            }
            catch (System.Exception ex)
            {
                products = new List<Products>();
                MessageBox.Show($"Не удалось загрузить товары из базы данных. Проверьте подключение и наличие базы FinalDemoExamKorepinVD.\n\n{ex.Message}",
                    "Ошибка загрузки данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (products == null) products = new List<Products>();
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (!isWindowReady || lvProducts == null)
            {
                return;
            }

            if (products == null || products.Count == 0)
            {
                lvProducts.ItemsSource = emptyProducts;
                return;
            }

            IEnumerable<Products> result = products;

            bool canSearch = App.UserRole == 1 || App.UserRole == 2;

            if (canSearch)
            {
                string searchText = txtSearch?.Text?.Trim().ToLower();
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    result = result.Where(p =>
                        (p.article ?? "").ToLower().Contains(searchText) ||
                        (p.name ?? "").ToLower().Contains(searchText) ||
                        (p.description ?? "").ToLower().Contains(searchText) ||
                        (p.Categories != null && p.Categories.name != null && p.Categories.name.ToLower().Contains(searchText)) ||
                        (p.Manufacturers != null && p.Manufacturers.name != null && p.Manufacturers.name.ToLower().Contains(searchText)) ||
                        (p.Suppliers != null && p.Suppliers.name != null && p.Suppliers.name.ToLower().Contains(searchText))
                    );
                }

                if (cmbDiscountFilter != null && cmbDiscountFilter.SelectedItem != null)
                {
                    string filter = (cmbDiscountFilter.SelectedItem as ComboBoxItem)?.Content?.ToString();
                    if (filter == "0-12,99%") result = result.Where(p => p.discount >= 0 && p.discount < 13);
                    else if (filter == "13-16,99%") result = result.Where(p => p.discount >= 13 && p.discount < 17);
                    else if (filter == "17% и более") result = result.Where(p => p.discount >= 17);
                }

                if (cmbSort != null && cmbSort.SelectedItem != null)
                {
                    string sort = (cmbSort.SelectedItem as ComboBoxItem)?.Content?.ToString();
                    if (sort == "Цена по возрастанию") result = result.OrderBy(p => p.price);
                    else if (sort == "Цена по убыванию") result = result.OrderByDescending(p => p.price);
                    else if (sort == "Количество по возрастанию") result = result.OrderBy(p => p.stock_quantity);
                    else if (sort == "Количество по убыванию") result = result.OrderByDescending(p => p.stock_quantity);
                }
            }

            lvProducts.ItemsSource = result.ToList();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilters();
        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyFilters();
        private void cmbDiscountFilter_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyFilters();

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            new AddEditWindow().ShowDialog();
            Refresh();
        }

        private void lvProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (App.UserRole != 2) return;
            if (lvProducts.SelectedItem is Products product)
            {
                new AddEditWindow(product).ShowDialog();
                Refresh();
            }
        }

        private void btnOrders_Click(object sender, RoutedEventArgs e) => new ListOrderWindow().ShowDialog();
    }
}