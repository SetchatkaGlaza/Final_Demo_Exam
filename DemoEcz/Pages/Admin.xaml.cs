using DemoEcz.Entities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DemoEcz.Pages
{
    public partial class Admin : Page
    {
        public Admin()
        {
            InitializeComponent();
            FIO.Text = App.CurentUser?.FIO ?? "";
            UpdateList();
        }

        private void UpdateList()
        {
            var list = App.ContextBD.Tovars.ToList();

            // Сортировка
            if (Sorts.SelectedIndex == 1)
                list = list.OrderBy(t => t.ostatok).ToList();
            else if (Sorts.SelectedIndex == 2)
                list = list.OrderByDescending(t => t.ostatok).ToList();
            else if (Sorts.SelectedIndex == 3)
                list = list.OrderBy(t => t.cena).ToList();
            else if (Sorts.SelectedIndex == 4)
                list = list.OrderByDescending(t => t.cena).ToList();

            // Поиск
            if (!string.IsNullOrEmpty(Poisk.Text))
               list = list.Where(t => (t.name ?? "").ToLower().Contains(Poisk.Text.ToLower()) ||
                                       (t.postaw ?? "").ToLower().Contains(Poisk.Text.ToLower()) ||
                                       (t.proizw ?? "").ToLower().Contains(Poisk.Text.ToLower()) ||
                                       (t.opisanie ?? "").ToLower().Contains(Poisk.Text.ToLower())).ToList();

            // Фильтр по скидке
            if (Filtrs.SelectedIndex == 1) // 0-12.99%
                list = list.Where(t => t.skidka >= 0 && t.skidka < 13).ToList();
            else if (Filtrs.SelectedIndex == 2) // 13-16.99%
                list = list.Where(t => t.skidka >= 13 && t.skidka < 17).ToList();
            else if (Filtrs.SelectedIndex == 3) // 17% и более
                list = list.Where(t => t.skidka >= 17).ToList();

            TovarVivod.ItemsSource = list;
        }

        private void Text_TextChanged(object s, RoutedEventArgs e) => UpdateList();
        private void FiltrsSelectionChanged(object s, SelectionChangedEventArgs e) => UpdateList();
        private void SortsSelectionChanged(object s, SelectionChangedEventArgs e) => UpdateList();
        private void Vhod_Click(object s, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void Dob_Click(object s, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditProductPage());
        }

        private void TovarVivodMouseLeftButtonUp(object s, MouseButtonEventArgs e)
        {
           if((s as Border)?.DataContext is Tovar product)
            NavigationService.Navigate(new AddEditProductPage(product));
        }
        private void UadlClick(object s, RoutedEventArgs e)
        {
            if (!((s as Button)?.Tag is Tovar product))
                return;
            if (MessageBox.Show($"Удалить \"{product.name}\"?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    // Проверка, есть ли товар в заказах
                    if (App.ContextBD.Zakazs.Any(z => z.art == product.art))
                    {
                        MessageBox.Show("Нельзя удалить товар, который присутствует в заказе!");
                        return;
                    }
                    App.ContextBD.Tovars.Remove(product);
                    App.ContextBD.SaveChanges();
                    UpdateList();
                    MessageBox.Show("Товар удален!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}");
                }
            }
        }
        private void PageLoaded(object s, RoutedEventArgs e) => UpdateList();
    }
}