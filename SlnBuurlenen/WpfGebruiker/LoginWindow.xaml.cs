using System.Data.SqlClient;
using System.Windows;
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
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            // variabelen declareren
            string email = txtEmail.Text;
            string paswoord = txtPassword.Password;

            string connStr = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=BuurlenenDB;Integrated Security=True";

            // nieuwe connectie maken
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                // Opent de databaseverbinding
                connection.Open();
                string queryEMPA = "SELECT COUNT(*) FROM [dbo].[Gebruiker] WHERE email = @Email AND paswoord = @Paswoord";
                using (SqlCommand command = new SqlCommand(queryEMPA, connection))
                {
                    // Parameters doorgeven
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Paswoord", paswoord);

                    int counter = (int)command.ExecuteScalar();

                    if (counter > 0)
                    {
                        queryEMPA = "SELECT * FROM [dbo].[Gebruiker] WHERE email = @Email AND paswoord = @Paswoord";
                        using (SqlCommand getUserCommand = new SqlCommand(queryEMPA, connection))
                        {
                            // Stelt opnieuw de waarden in
                            getUserCommand.Parameters.AddWithValue("@Email", email);
                            getUserCommand.Parameters.AddWithValue("@Paswoord", paswoord);

                            // Voert de query uit en krijgt een SqlDataReader-object terug
                            using (SqlDataReader rdr = getUserCommand.ExecuteReader())
                            {
                                if (rdr.Read())
                                {
                                    // Leest de kolomwaarden van de huidige rij
                                    int userId = (int)rdr["id"];
                                    string voorNaam = (string)rdr["voornaam"];
                                    string achterNaam = (string)rdr["achternaam"];

                                    MainWindow mainWindow = new MainWindow(userId, voorNaam, achterNaam);
                                    mainWindow.Show();
                                    Close();
                                }
                            }
                        }
                    }
                    else
                    {
                        lblError.Content = "E-mail of wachtwoord is onjuist!";
                    }
                }
            }
        }
    }
}
