using FinalDemoExam.Entity;
using System;
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
        private List<Products> allProducts;

        public MainWindow()
        {
            InitializeComponent();
            txtUserFullName.Text = App.UserFullName;
            SetPermissions();
            Refresh();
        }

        private void SetPermissions()
        {
            if (App.UserRole == "Гость" || App.UserRole == "Авторизированный клиент")
            {
                btnAdd.Visibility = Visibility.Collapsed;
                btnOrders.Visibility = Visibility.Collapsed;
            }
            else if (App.UserRole == "Менеджер")
            {
                btnAdd.Visibility = Visibility.Collapsed;
                btnOrders.Visibility = Visibility.Visible;
            }
            else if (App.UserRole == "Администратор")
            {
                btnAdd.Visibility = Visibility.Visible;
                btnOrders.Visibility = Visibility.Visible;
            }
        }

        public void Refresh()
        {
            allProducts = App.DB.Products.Include(p => p.Categories).Include(p => p.Manufacturers).Include(p => p.Suppliers).Include(p => p.Units).ToList();
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (allProducts == null)
            {
                return;
            }

            var query = allProducts.AsEnumerable();

            string search = txtSearch.Text?.ToLower();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                    p.name != null && p.name.ToLower().Contains(search) ||
                    p.description != null && p.description.ToLower().Contains(search) ||
                    p.Categories != null && p.Categories.name != null && p.Categories.name.ToLower().Contains(search) ||
                    p.Manufacturers != null && p.Manufacturers.name != null && p.Manufacturers.name.ToLower().Contains(search) ||
                    p.Suppliers != null && p.Suppliers.name != null && p.Suppliers.name.ToLower().Contains(search) ||
                    p.article != null && p.article.ToLower().Contains(search)
                );
            }

            if (cmbDiscountFilter != null && cmbDiscountFilter.SelectedItem != null && cmbDiscountFilter.SelectedItem is ComboBoxItem discountItem)
            {
                string discountFilter = discountItem.Content.ToString();
                if (discountFilter == "0-12,99%")
                    query = query.Where(p => p.discount >= 0 && p.discount < 13);
                else if (discountFilter == "13-16,99%")
                    query = query.Where(p => p.discount >= 13 && p.discount < 17);
                else if (discountFilter == "17% и более")
                    query = query.Where(p => p.discount >= 17);
            }

            if (cmbSort != null && cmbSort.SelectedItem != null && cmbSort.SelectedItem is ComboBoxItem sortItem)
            {
                string sort = sortItem.Content.ToString();
                if (sort == "Цена по возрастанию")
                    query = query.OrderBy(p => p.price);
                else if (sort == "Цена по убыванию")
                    query = query.OrderByDescending(p => p.price);
                else if (sort == "Количество по возрастанию")
                    query = query.OrderBy(p => p.stock_quantity);
                else if (sort == "Количество по убыванию")
                    query = query.OrderByDescending(p => p.stock_quantity);
            }

            lvProducts.ItemsSource = query.ToList();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cmbDiscountFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEditWindow addEdit = new AddEditWindow();
            addEdit.ShowDialog();
            Refresh();
        }

        private void lvProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (App.UserRole != "Администратор") return;
            if (lvProducts.SelectedItem is Products product)
            {
                AddEditWindow addEdit = new AddEditWindow(product);
                addEdit.ShowDialog();
                Refresh();
            }
        }

        private void btnOrders_Click(object sender, RoutedEventArgs e)
        {
            ListOrderWindow orders = new ListOrderWindow();
            orders.ShowDialog();
        }
    }
}