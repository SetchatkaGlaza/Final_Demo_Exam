using DemoEcz.Entities;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DemoEcz.Pages
{
    public partial class AddEditProductPage : Page
    {
    //    private Tovar _product;
    //    private string _imagePath;
    //    private bool _isNew = true;

        public AddEditProductPage()
        {
            InitializeComponent();
            //_isNew = true;
            //_product = new Tovar();
        }

        public AddEditProductPage(Tovar product)
        {
            InitializeComponent();
            //_isNew = false;
            //_product = product;
            //LoadData();
        }

        private void LoadData()
        {
            //TBoxArticle.Text = _product.art;
            //TBoxName.Text = _product.name;
            //TBoxDescription.Text = _product.opisanie;
            //TBoxCost.Text = _product.cena.ToString();
            //TBoxQuantity.Text = _product.ostatok?.ToString() ?? "0";
            //TBoxDiscount.Text = _product.skidka?.ToString() ?? "0";
            //TBoxEdIzm.Text = _product.ed_izm ?? "шт";
            //TBoxProizw.Text = _product.proizw ?? "";
            //TBoxKat.Text = _product.kat?.ToString() ?? "1";

            //// Установка поставщика в ComboBox
            //if (!string.IsNullOrEmpty(_product.postaw))
            //{
            //    foreach (ComboBoxItem item in CBoxPostav.Items)
            //    {
            //        if (item.Content.ToString() == _product.postaw)
            //        {
            //            CBoxPostav.SelectedItem = item;
            //            break;
            //        }
            //    }
            //}

            //if (!string.IsNullOrEmpty(_product.foto))
            //{
            //    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", _product.foto);
            //    if (File.Exists(path))
            //        ImageProduct.Source = new BitmapImage(new Uri(path));
            //}
        }

        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            //var dialog = new OpenFileDialog
            //{
            //    Filter = "Изображения|*.png;*.jpg;*.jpeg;*.bmp;*.gif"
            //};
            //if (dialog.ShowDialog() == true)
            //{
            //    _imagePath = dialog.FileName;
            //    ImageProduct.Source = new BitmapImage(new Uri(_imagePath));
            //}
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
        //    try
        //    {
        //        // Валидация
        //        if (string.IsNullOrWhiteSpace(TBoxArticle.Text))
        //        {
        //            MessageBox.Show("Заполните артикул!", "Предупреждение",
        //                MessageBoxButton.OK, MessageBoxImage.Warning);
        //            return;
        //        }

        //        if (string.IsNullOrWhiteSpace(TBoxName.Text))
        //        {
        //            MessageBox.Show("Заполните название товара!", "Предупреждение",
        //                MessageBoxButton.OK, MessageBoxImage.Warning);
        //            return;
        //        }

        //        if (!int.TryParse(TBoxCost.Text, out int cost) || cost < 0)
        //        {
        //            MessageBox.Show("Цена должна быть положительным числом!", "Предупреждение",
        //                MessageBoxButton.OK, MessageBoxImage.Warning);
        //            return;
        //        }

        //        if (!int.TryParse(TBoxQuantity.Text, out int quantity) || quantity < 0)
        //        {
        //            MessageBox.Show("Количество должно быть положительным числом!", "Предупреждение",
        //                MessageBoxButton.OK, MessageBoxImage.Warning);
        //            return;
        //        }

        //        // Заполнение данных
        //        _product.art = TBoxArticle.Text;
        //        _product.name = TBoxName.Text;
        //        _product.opisanie = TBoxDescription.Text;
        //        _product.cena = cost;
        //        _product.ed_izm = TBoxEdIzm.Text;
        //        _product.skidka = int.TryParse(TBoxDiscount.Text, out int d) ? d : 0;
        //        _product.ostatok = quantity;
        //        _product.proizw = TBoxProizw.Text;
        //        _product.kat = int.TryParse(TBoxKat.Text, out int k) ? k : 1;

        //        // Поставщик из ComboBox
        //        if (CBoxPostav.SelectedItem is ComboBoxItem postavItem)
        //            _product.postaw = postavItem.Content.ToString();
        //        else
        //            _product.postaw = "Kari"; // значение по умолчанию

        //        // Сохранение фото
        //        if (!string.IsNullOrEmpty(_imagePath))
        //        {
        //            string assets = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");
        //            Directory.CreateDirectory(assets);
        //            string fileName = Path.GetFileName(_imagePath);
        //            string dest = Path.Combine(assets, fileName);

        //            // Удаление старого фото если оно есть
        //            if (!_isNew && !string.IsNullOrEmpty(_product.foto))
        //            {
        //                string oldPath = Path.Combine(assets, _product.foto);
        //                if (File.Exists(oldPath) && oldPath != dest)
        //                    File.Delete(oldPath);
        //            }

        //            File.Copy(_imagePath, dest, true);
        //            _product.foto = fileName;
        //        }

        //        if (_isNew)
        //            App.ContextBD.Tovars.Add(_product);

        //        App.ContextBD.SaveChanges();
        //        MessageBox.Show("Товар успешно сохранен!", "Информация",
        //            MessageBoxButton.OK, MessageBoxImage.Information);
        //        NavigationService?.GoBack();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка при сохранении: {ex.Message}",
        //            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => NavigationService?.GoBack();
    }
}