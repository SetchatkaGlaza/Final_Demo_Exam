using FinalDemoExam.Entity;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace FinalDemoExam
{
    /// <summary>
    /// Логика взаимодействия для AddEditWindow.xaml
    /// </summary>
    public partial class AddEditWindow : Window
    {
        private Products product;
        private bool isEdit;
        private string selectedImagePath;
        public AddEditWindow(Products p = null)
        {
            InitializeComponent();
            isEdit = p != null;
            product = p ?? new Products();

            cmbCategory.ItemsSource = App.DB.Categories.ToList();
            cmbSupplier.ItemsSource = App.DB.Suppliers.ToList();
            cmbUnit.ItemsSource = App.DB.Units.ToList();

            if (isEdit)
            {
                txtArticle.Text = product.article;
                txtName.Text = product.name;
                cmbCategory.SelectedValue = product.category_id;
                txtDescription.Text = product.description;
                txtManufacturer.Text = product.Manufacturers.name;
                cmbSupplier.SelectedValue = product.supplier_id;
                txtPrice.Text = product.price.ToString();
                cmbUnit.SelectedValue = product.unit_id;
                txtStock.Text = product.stock_quantity.ToString();
                txtDiscount.Text = product.discount.ToString();

                if (!string.IsNullOrEmpty(product.photo_path) && File.Exists(product.photo_path))
                {
                    imgProduct.Source = new BitmapImage(new Uri(product.photo_path, UriKind.RelativeOrAbsolute));
                }
            }
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.jpg;*.png;*.jpeg)|*.jpg;*.png;*.jpeg";
            if (dlg.ShowDialog() == true)
            {
                selectedImagePath = dlg.FileName;
                imgProduct.Source = new BitmapImage(new Uri(dlg.FileName));
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Введите наименование товара!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
                {
                    MessageBox.Show("Цена должна быть положительным числом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
                {
                    MessageBox.Show("Количество не может быть отрицательным!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!decimal.TryParse(txtDiscount.Text, out decimal discount) || discount < 0)
                {
                    MessageBox.Show("Скидка не может быть отрицательной!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                product.name = txtName.Text;
                product.category_id = (int)cmbCategory.SelectedValue;
                product.description = txtDescription.Text;

                string manufacturerName = txtManufacturer.Text;
                var manufacturer = App.DB.Manufacturers.FirstOrDefault(m => m.name == manufacturerName);
                if (manufacturer == null)
                {
                    manufacturer = new Manufacturers { name = manufacturerName };
                    App.DB.Manufacturers.Add(manufacturer);
                    App.DB.SaveChanges();
                }
                product.manufacturer_id = manufacturer.id;

                product.supplier_id = (int)cmbSupplier.SelectedValue;
                product.price = price;
                product.unit_id = (int)cmbUnit.SelectedValue;
                product.stock_quantity = stock;
                product.discount = discount;

                if (!string.IsNullOrEmpty(selectedImagePath))
                {
                    string fileName = $"{product.article}_{DateTime.Now.Ticks}{Path.GetExtension(selectedImagePath)}";
                    string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                    File.Copy(selectedImagePath, destPath, true);
                    product.photo_path = destPath;
                }

                if (!isEdit)
                {
                    int maxId = App.DB.Products.Any() ? App.DB.Products.Max(p => int.Parse(p.article.Substring(1))) : 0;
                    product.article = $"P{maxId + 1}";
                    App.DB.Products.Add(product);
                }

                App.DB.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
