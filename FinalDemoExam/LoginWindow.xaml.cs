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
            var user = App.DB.Users.FirstOrDefault(u => u.login == txtLogin.Text && u.password == txtPassword.Password);
            if (user != null)
            {
                App.UserId = user.id;
                App.UserFullName = user.full_name;
                App.UserRole = user.role_id;
                new MainWindow().Show();
                Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }

        private void BtnGuest_Click(object sender, RoutedEventArgs e)
        {
            App.UserId = 0;
            App.UserFullName = "Гость";
            App.UserRole = 3;
            new MainWindow().Show();
            Close();
        }
    }
}