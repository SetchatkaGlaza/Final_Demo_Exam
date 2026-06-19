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
    /// Логика взаимодействия для Gost.xaml
    /// </summary>
    public partial class Gost : Page
    {

        public Gost()
        {
            InitializeComponent();
            TovarVivod.ItemsSource = App.ContextBD.Tovars.ToList();

        }

        private void Vhod_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
