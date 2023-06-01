using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using MyClassLibrary;
namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            txtEmail.Text = "sam@odisee.be";
            txtPassword.Password = "test123";
        }

        // button om in te logen in de MainWindow
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;

            Gebruiker gebruikerLogin = Gebruiker.LogConn(email, SHA256Hash(password));
            if (gebruikerLogin != null)
            {
                MainWindow mainWindow = new MainWindow(gebruikerLogin);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                lblError.Content = "E-mail of wachtwoord is onjuist!";
            }
        }

        // PasswordHashing
        public string SHA256Hash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}