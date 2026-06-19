using FinalDemoExam.Entity;
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
            var user = App.DB.Users.FirstOrDefault(u => u.login == txtLogin.Text && u.password == txtPassword.Password);
            if (user != null)
            {
                App.UserId = user.id;
                App.UserFullName = user.full_name;
                App.UserRole = user.Roles.name;

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