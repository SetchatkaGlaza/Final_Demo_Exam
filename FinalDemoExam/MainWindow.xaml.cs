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
        private List<Products> products = new List<Products>();
        private bool CanManage => App.UserRole == "Администратор";
        private bool CanSearch => App.UserRole == "Администратор" || App.UserRole == "Менеджер";

        public MainWindow()
        {
            InitializeComponent();
            txtUserFullName.Text = App.UserFullName;
            btnAdd.Visibility = CanManage ? Visibility.Visible : Visibility.Collapsed;
            btnOrders.Visibility = CanSearch ? Visibility.Visible : Visibility.Collapsed;
            filtersPanel.Visibility = CanSearch ? Visibility.Visible : Visibility.Collapsed;
            Refresh();
        }

        public void Refresh()
        {
            products = App.DB.Products.Include(p => p.Categories).Include(p => p.Manufacturers).Include(p => p.Suppliers).Include(p => p.Units).ToList();
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            IEnumerable<Products> result = products;
            if (CanSearch) result = Sort(Filter(Search(result)));
            lvProducts.ItemsSource = result.ToList();
        }

        private IEnumerable<Products> Search(IEnumerable<Products> source)
        {
            string text = txtSearch.Text?.ToLower();
            if (string.IsNullOrWhiteSpace(text)) return source;
            return source.Where(p => (p.article + " " + p.name + " " + p.description + " " + p.Categories?.name + " " + p.Manufacturers?.name + " " + p.Suppliers?.name + " " + p.Units?.name).ToLower().Contains(text));
        }

        private IEnumerable<Products> Filter(IEnumerable<Products> source)
        {
            string value = ((ComboBoxItem)cmbDiscountFilter.SelectedItem)?.Content.ToString();
            if (value == "0-12,99%") return source.Where(p => p.discount >= 0 && p.discount < 13);
            if (value == "13-16,99%") return source.Where(p => p.discount >= 13 && p.discount < 17);
            if (value == "17% и более") return source.Where(p => p.discount >= 17);
            return source;
        }

        private IEnumerable<Products> Sort(IEnumerable<Products> source)
        {
            string value = ((ComboBoxItem)cmbSort.SelectedItem)?.Content.ToString();
            if (value == "Цена по возрастанию") return source.OrderBy(p => p.price);
            if (value == "Цена по убыванию") return source.OrderByDescending(p => p.price);
            if (value == "Количество по возрастанию") return source.OrderBy(p => p.stock_quantity);
            if (value == "Количество по убыванию") return source.OrderByDescending(p => p.stock_quantity);
            return source;
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
            if (!CanManage || !(lvProducts.SelectedItem is Products product)) return;
            new AddEditWindow(product).ShowDialog();
            Refresh();
        }

        private void btnOrders_Click(object sender, RoutedEventArgs e) => new ListOrderWindow().ShowDialog();
    }
}
