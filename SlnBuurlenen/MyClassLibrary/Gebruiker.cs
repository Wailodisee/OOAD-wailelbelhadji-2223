using System;
using System.Configuration;
using System.Data.SqlClient;

namespace MyClassLibrary
{
    public enum GeslachtsEnum : byte
    {
        Man = 0,
        Vrouw = 1
    }
    public class Gebruiker
    {
        public int Id { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string Email { get; set; }
        public string Paswoord { get; set; }
        public DateTime Aanmaakdatum { get; set; }
        public byte[] Profielfoto { get; set; }
        public GeslachtsEnum Geslacht { get; set; }

        // Leest de gegevens van de database + maak een Gebruiker-object
        public Gebruiker(SqlDataReader rdr)
        {
            Id = Convert.ToInt32(rdr["id"]);
            Voornaam = Convert.ToString(rdr["voornaam"]);
            Achternaam = Convert.ToString(rdr["achternaam"]);
            Email = Convert.ToString(rdr["email"]);
            Paswoord = Convert.ToString(rdr["paswoord"]);
            Aanmaakdatum = Convert.ToDateTime(rdr["aanmaakdatum"]);
            Profielfoto = rdr["profielfoto"] as byte[];
            Geslacht = (GeslachtsEnum)Convert.ToByte(rdr["geslacht"]);
        }

        // Voert een query om een gebruiker te vinden op basis van e-mail en wachtwoord + return een Gebruiker-object
        public static Gebruiker LogConn(string email, string password)
        {
            using (SqlConnection connectionSQL = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                connectionSQL.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Gebruiker WHERE email = @email AND paswoord = @password", connectionSQL);
                command.Parameters.AddWithValue("email", email);
                command.Parameters.AddWithValue("password", password);
                SqlDataReader rdrr = command.ExecuteReader();

                if (rdrr.Read())
                {
                    return new Gebruiker(rdrr);
                }
                else
                {
                    return null;
                }
            }
        }

        // Voert een query om een gebruiker te vinden op basis van ID + return een Gebruiker-object 
        public static Gebruiker GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Gebruiker WHERE id = @id", connection);
                command.Parameters.AddWithValue("@id", id);
                SqlDataReader rdr = command.ExecuteReader();

                Gebruiker gebruiker = null;

                if (rdr.Read())
                {
                    gebruiker = new Gebruiker(rdr);
                }

                return gebruiker;
            }
        }
    }
}
