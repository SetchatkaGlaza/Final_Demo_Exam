using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace DemoEcz.Pages
{
    public partial class Vhod : Page
    {
        public Vhod()
        {
            InitializeComponent();
        }

        private void Vhod_Click(object sender, RoutedEventArgs e)
        {
            var User = App.ContextBD.Users.FirstOrDefault(u => u.log == Login.Text && u.pass == Pass.Password);

            if (User != null)
            {
                App.CurentUser = User;

                if (User.rol == 1)
                {
                    NavigationService.Navigate(new Polz());
                }
                else if (User.rol == 2)
                {
                    NavigationService.Navigate(new Meneg());
                }
                else if (User.rol == 3)
                {
                    NavigationService.Navigate(new Admin());
                }
                else
                {
                    MessageBox.Show("Ошибка");
                }
            }
            else
            {
                MessageBox.Show("Перепроверьте данные пользователя");
            }
        }

        private void VhodGost_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Gost());
        }
    }
}