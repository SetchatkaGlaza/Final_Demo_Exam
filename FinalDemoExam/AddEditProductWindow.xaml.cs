using FinalDemoExam.Entity;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Path = System.IO.Path;

namespace FinalDemoExam
{
    public partial class AddEditWindow : Window
    {
        private readonly Products product;
        private readonly bool isEdit;
        private string selectedImagePath;

        public AddEditWindow(Products selectedProduct = null)
        {
            InitializeComponent();
            product = selectedProduct ?? new Products();
            isEdit = selectedProduct != null;
            cmbCategory.ItemsSource = App.DB.Categories.ToList();
            cmbSupplier.ItemsSource = App.DB.Suppliers.ToList();
            cmbUnit.ItemsSource = App.DB.Units.ToList();
            if (isEdit) FillForm();
        }

        private void FillForm()
        {
            txtArticle.Text = product.article;
            txtName.Text = product.name;
            txtDescription.Text = product.description;
            txtManufacturer.Text = product.Manufacturers?.name;
            txtPrice.Text = product.price.ToString("0.##");
            txtStock.Text = product.stock_quantity.ToString();
            txtDiscount.Text = product.discount.ToString("0.##");
            cmbCategory.SelectedValue = product.category_id;
            cmbSupplier.SelectedValue = product.supplier_id;
            cmbUnit.SelectedValue = product.unit_id;
            btnDelete.Visibility = Visibility.Visible;
            imgProduct.Source = new BitmapImage(new Uri(product.DisplayPhotoPath, UriKind.RelativeOrAbsolute));
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog { Filter = "Изображения|*.jpg;*.jpeg;*.png" };
            if (dialog.ShowDialog() != true) return;
            selectedImagePath = dialog.FileName;
            imgProduct.Source = new BitmapImage(new Uri(selectedImagePath));
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ReadForm(out decimal price, out int stock, out decimal discount)) return;
            if (!isEdit)
            {
                product.article = GetNextArticle();
                App.DB.Products.Add(product);
            }

            product.name = txtName.Text.Trim();
            product.description = txtDescription.Text.Trim();
            product.category_id = (int)cmbCategory.SelectedValue;
            product.supplier_id = (int)cmbSupplier.SelectedValue;
            product.unit_id = (int)cmbUnit.SelectedValue;
            product.manufacturer_id = GetManufacturerId(txtManufacturer.Text.Trim());
            product.price = price;
            product.stock_quantity = stock;
            product.discount = discount;
            try
            {
                product.photo_path = SaveImage(product.photo_path);
                App.DB.SaveChanges();
                DialogResult = true;
            }
            catch (Exception ex)
            {
                Warn("Не удалось сохранить товар: " + ex.Message);
            }
        }

        private bool ReadForm(out decimal price, out int stock, out decimal discount)
        {
            price = discount = 0;
            stock = 0;
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtManufacturer.Text)) return Warn("Заполните наименование и производителя.");
            if (cmbCategory.SelectedValue == null || cmbSupplier.SelectedValue == null || cmbUnit.SelectedValue == null) return Warn("Выберите категорию, поставщика и единицу измерения.");
            if (!decimal.TryParse(txtPrice.Text, out price) || price < 0) return Warn("Цена должна быть неотрицательным числом.");
            if (!int.TryParse(txtStock.Text, out stock) || stock < 0) return Warn("Количество должно быть неотрицательным целым числом.");
            if (!decimal.TryParse(txtDiscount.Text, out discount) || discount < 0) return Warn("Скидка должна быть неотрицательным числом.");
            return true;
        }

        private int GetManufacturerId(string name)
        {
            var manufacturer = App.DB.Manufacturers.FirstOrDefault(m => m.name == name);
            if (manufacturer != null) return manufacturer.id;
            manufacturer = new Manufacturers { name = name };
            App.DB.Manufacturers.Add(manufacturer);
            App.DB.SaveChanges();
            return manufacturer.id;
        }

        private string GetNextArticle()
        {
            int number = 1;
            string article;
            do article = $"P{number++:0000}";
            while (App.DB.Products.Any(p => p.article == article));
            return article;
        }

        private string SaveImage(string oldPath)
        {
            if (string.IsNullOrEmpty(selectedImagePath)) return oldPath;
            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            Directory.CreateDirectory(folder);
            string newPath = Path.Combine(folder, $"{product.article}_{DateTime.Now.Ticks}{Path.GetExtension(selectedImagePath)}");
            BitmapFrame source = BitmapFrame.Create(new Uri(selectedImagePath));
            double scale = Math.Min(300d / source.PixelWidth, 200d / source.PixelHeight);
            BitmapSource image = new TransformedBitmap(source, new System.Windows.Media.ScaleTransform(Math.Min(1, scale), Math.Min(1, scale)));
            BitmapEncoder encoder = Path.GetExtension(newPath).ToLower() == ".png" ? (BitmapEncoder)new PngBitmapEncoder() : new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (var file = File.Create(newPath)) encoder.Save(file);
            if (!string.IsNullOrEmpty(oldPath) && oldPath != newPath && File.Exists(oldPath)) File.Delete(oldPath);
            return newPath;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!isEdit || product.OrderItems.Any())
            {
                Warn("Товар присутствует в заказе, поэтому удалить его нельзя.");
                return;
            }
            if (MessageBox.Show("Удалить выбранный товар?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            try
            {
                App.DB.Products.Remove(product);
                App.DB.SaveChanges();
                DialogResult = true;
            }
            catch (Exception ex)
            {
                Warn("Не удалось удалить товар: " + ex.Message);
            }
        }

        private bool Warn(string text)
        {
            MessageBox.Show(text, "Проверьте данные", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
