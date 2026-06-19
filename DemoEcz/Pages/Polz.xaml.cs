using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoEcz.Pages
{
    /// <summary>
    /// Логика взаимодействия для Polz.xaml
    /// </summary>
    public partial class Polz : Page
    {
        public Polz()
        {
            InitializeComponent();
            TovarVivod.ItemsSource = App.ContextBD.Tovars.ToList();
            if (App.CurentUser != null)
            {
                var User = App.CurentUser;

                FIO.Text = User.FIO;

            }
        }



        private void Vhod_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
