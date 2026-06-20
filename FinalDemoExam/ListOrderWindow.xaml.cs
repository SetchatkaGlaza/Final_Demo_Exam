using FinalDemoExam.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinalDemoExam
{
    public partial class ListOrderWindow : Window
    {
        public ListOrderWindow()
        {
            InitializeComponent();
            txtUserFullName.Text = App.UserFullName;
            if (App.UserRole == 1)
            {
                btnAdd.Visibility = Visibility.Visible;
            }
            Refresh();
        }

        private void Refresh()
        {
            lvOrders.ItemsSource = App.DB.Orders.ToList();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEditOrderWindow addEdit = new AddEditOrderWindow();
            addEdit.ShowDialog();
            Refresh();
        }

        private void lvOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (App.UserRole != 1) return;
            if (lvOrders.SelectedItem is Orders order)
            {
                AddEditOrderWindow addEdit = new AddEditOrderWindow(order);
                addEdit.ShowDialog();
                Refresh();
            }
        }
    }
}