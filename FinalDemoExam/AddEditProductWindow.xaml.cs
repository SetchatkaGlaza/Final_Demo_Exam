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
        private Products product;
        private bool isEdit;
        private string selectedImagePath;


        public AddEditWindow(Products selectedProduct = null)
        {
            InitializeComponent();
            product = selectedProduct ?? new Products();
            isEdit = selectedProduct != null;

            cmbCategory.ItemsSource = App.DB.Categories.ToList();
            cmbSupplier.ItemsSource = App.DB.Suppliers.ToList();
            cmbUnit.ItemsSource = App.DB.Units.ToList();

            if (isEdit)
                FillForm();
            else
            {
                txtArticle.Text = "Будет создан автоматически";
                txtDiscount.Text = "0";
                txtStock.Text = "0";
                txtPhotoPath.Text = "Файл не выбран";
            }
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

            if (!string.IsNullOrEmpty(product.photo_path) && File.Exists(product.photo_path))
                txtPhotoPath.Text = Path.GetFileName(product.photo_path);
            else
                txtPhotoPath.Text = "Файл не выбран";
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog { Filter = "Изображения|*.jpg;*.jpeg;*.png" };
            if (dialog.ShowDialog() == true)
            {
                selectedImagePath = dialog.FileName;
                txtPhotoPath.Text = Path.GetFileName(selectedImagePath);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Заполните наименование товара.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtManufacturer.Text))
            {
                MessageBox.Show("Заполните производителя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cmbCategory.SelectedValue == null)
            {
                MessageBox.Show("Выберите категорию.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cmbSupplier.SelectedValue == null)
            {
                MessageBox.Show("Выберите поставщика.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cmbUnit.SelectedValue == null)
            {
                MessageBox.Show("Выберите единицу измерения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Цена должна быть неотрицательным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Количество должно быть неотрицательным целым числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtDiscount.Text, out decimal discount) || discount < 0 || discount > 100)
            {
                MessageBox.Show("Скидка должна быть числом от 0 до 100.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

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
            product.manufacturer_id = GetOrCreateManufacturer(txtManufacturer.Text.Trim());
            product.price = price;
            product.stock_quantity = stock;
            product.discount = discount;
            product.photo_path = SaveImage();

            App.DB.SaveChanges();
            DialogResult = true;
            Close();
        }

        private int GetOrCreateManufacturer(string name)
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

        private string SaveImage()
        {
            if (string.IsNullOrEmpty(selectedImagePath)) return product.photo_path;

            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            Directory.CreateDirectory(folder);
            string newPath = Path.Combine(folder, $"{product.article}_{DateTime.Now.Ticks}{Path.GetExtension(selectedImagePath)}");
            File.Copy(selectedImagePath, newPath, true);
            return newPath;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!isEdit) return;

            if (product.OrderItems.Any())
            {
                MessageBox.Show("Товар присутствует в заказе, удалить нельзя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Удалить товар?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                App.DB.Products.Remove(product);
                App.DB.SaveChanges();
                DialogResult = true;
                Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}