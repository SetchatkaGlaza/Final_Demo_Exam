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
    /// Логика взаимодействия для Meneg.xaml
    /// </summary>
    public partial class Meneg : Page
    {
        List<Entities.Tovar> Tov = App.ContextBD.Tovars.ToList();

        public Meneg()
        {
            InitializeComponent();

            if (App.CurentUser != null)
            {
                var User = App.CurentUser;

                FIO.Text = User.FIO;
            }
            TovarVivod.ItemsSource = Tov;
        }

        private void Vhod_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        public void UpChanged()
        {
            Tov = App.ContextBD.Tovars.ToList();

            // сортировка
            if (Sorts.SelectedIndex == 1)
            {
                Tov = Tov.OrderBy(t => t.ostatok).ToList();
            }
            else if (Sorts.SelectedIndex == 2)
            {
                Tov = Tov.OrderByDescending(t => t.ostatok).ToList();
            }

            // поиск
            Tov = Tov.Where(t => t.name.ToLower().Contains(Text.Text.ToLower())
                || t.postaw.ToLower().Contains(Text.Text.ToLower())
                || t.proizw.ToLower().Contains(Text.Text.ToLower())
                ).ToList();
            // фильтрация

            if (Filtrs.SelectedIndex == 1)
            {
                Tov = Tov.Where(t => t.postaw == "Kari").ToList();
            }
            else if (Filtrs.SelectedIndex == 2)
            {
                Tov = Tov.Where(t => t.postaw == "Обувь для вас").ToList();
            }

            TovarVivod.ItemsSource = Tov;
        }

        private void TextSelectionChanged(object sender, RoutedEventArgs e)
        {
            UpChanged();
        }

        private void FiltrsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpChanged();
        }

        private void SortsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpChanged();
        }
    }
}
