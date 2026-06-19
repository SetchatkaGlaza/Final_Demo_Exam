using FinalDemoExam.Entity;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace FinalDemoExam
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var login = txtLogin.Text?.Trim();
            var password = txtPassword.Password;

            var user = App.DB.Users.Include("Roles").FirstOrDefault(u => u.login == login && u.password == password);
            if (user != null)
            {
                App.UserId = user.id;
                App.UserFullName = user.full_name ?? "Пользователь";
                App.UserRole = user.Roles?.name ?? string.Empty;

                MainWindow main = new MainWindow();
                main.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnGuest_Click(object sender, RoutedEventArgs e)
        {
            App.UserId = 0;
            App.UserFullName = "Гость";
            App.UserRole = "Гость";

            MainWindow main = new MainWindow();
            main.Show();
            Close();
        }
    }
}