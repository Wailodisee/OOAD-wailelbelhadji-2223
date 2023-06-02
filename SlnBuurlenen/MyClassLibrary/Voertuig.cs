using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MyClassLibrary
{
    public class Voertuig
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Beschrijving { get; set; }
        public int Bouwjaar { get; set; }
        public string Merk { get; set; }
        public string Model { get; set; }
        public int Type { get; set; }
        public Gebruiker Eigenaar_id { get; set; }

        public Voertuig()
        {
            // HIer komt de voertuigen
        }

        // Maak een lijst van Voertuig-objecten op basis van de waarde van het kolom "Type" in de database
        public static List<Voertuig> GetAll()
        {
            List<Voertuig> mijnVoertuigen = new List<Voertuig>();

            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Voertuig";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader rdrr = command.ExecuteReader())
                    {
                        while (rdrr.Read())
                        {
                            int typeVoertuig = Convert.ToInt32(rdrr["Type"]);
                            if (typeVoertuig == 1)
                            {
                                mijnVoertuigen.Add(new MotorVoertuig(rdrr));
                            }
                            else if (typeVoertuig == 2)
                            {
                                mijnVoertuigen.Add(new GetrokkenVoertuig(rdrr));
                            }
                            else
                            {
                                mijnVoertuigen.Add(new Voertuig(rdrr));
                            }
                        }
                    }
                }
            }
            return mijnVoertuigen;
        }

        // Initialiseert een Voertuig-object met behulp van de waarden uit de database 
        public Voertuig(SqlDataReader rdr)
        {
            Id = Convert.ToInt32(rdr["Id"]);

            Naam = Convert.ToString(rdr["Naam"]);

            Beschrijving = Convert.ToString(rdr["Beschrijving"]);

            Bouwjaar = Convert.ToInt32(rdr["Bouwjaar"]);

            Merk = Convert.ToString(rdr["Merk"]);

            Model = Convert.ToString(rdr["Model"]);

            Type = Convert.ToInt32(rdr["Type"]);

            Eigenaar_id = Gebruiker.GetById(Convert.ToInt32(rdr["Eigenaar_Id"]));
        }

        // Haalt een voertuig uit de database + retourneert een specifiek subklasse-object
        public static Voertuig CatchIdOfVoertuig(int idVoertuig)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Voertuig WHERE Id = @id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@id", SqlDbType.Int).Value = idVoertuig;

                using (SqlDataReader rdrr = command.ExecuteReader())
                {
                    if (rdrr.Read())
                    {
                        int typeVoertuig = (int)rdrr["Type"];

                        switch (typeVoertuig)
                        {
                            case 1:
                                return new MotorVoertuig(rdrr);

                            case 2:
                                return new GetrokkenVoertuig(rdrr);

                            default:
                                return new Voertuig(rdrr);
                        }
                    }
                }
            }

            return null;
        }

        // Haalt een lijst van voertuigen op uit de DB waarbij de eigenaar hetzelfde ID heeft als het opgegeven gebruikerID
        public static List<Voertuig> CatchId(int idGebruiker)
        {
            List<Voertuig> mijnVoertuig = new List<Voertuig>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM Voertuig WHERE Eigenaar_Id=@eigenaar_id", connection);
                command.Parameters.AddWithValue("@eigenaar_id", idGebruiker);

                SqlDataReader rdrr = command.ExecuteReader();

                if (rdrr.HasRows)
                {
                    rdrr.Read();

                    do
                    {
                        int mijnType = Convert.ToInt32(rdrr["Type"]);
                        if (mijnType == 1)
                        {
                            mijnVoertuig.Add(new MotorVoertuig(rdrr));
                        }
                        else if (mijnType == 2)
                        {
                            mijnVoertuig.Add(new GetrokkenVoertuig(rdrr));
                        }
                        else
                        {
                            mijnVoertuig.Add(new Voertuig(rdrr));
                        }
                    } while (rdrr.Read());
                }
            }
            return mijnVoertuig;
        }

        // Verwijdert een record uit de database Voertuig tabel op basis van het opgegeven id
        public static void DeleteRecord(int id)
        {
            string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();

                string query = "DELETE FROM Voertuig WHERE Id=@id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Maakt een record aan in de DB
        public static int CreateRecord(Voertuig idVoertuig)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                connection.Open();
                SqlCommand commandText = new SqlCommand(
                    @"INSERT INTO Voertuig (Naam, Beschrijving, Bouwjaar, Merk, Model, Type, Transmissie, Brandstof, Gewicht, Maxbelasting, Afmetingen, Geremd, Eigenaar_Id) VALUES (@name, @description, @bouwjaar, @merk, @model, @type, @transmissie, @brandstof, @gewicht, @maxbelasting, @afmetingen, @geremd, @owner); SELECT SCOPE_IDENTITY();", connection);

                commandText.Parameters.AddWithValue("@owner", idVoertuig.Eigenaar_id.Id);
                commandText.Parameters.AddWithValue("@name", idVoertuig.Naam);

                commandText.Parameters.AddWithValue("@description", idVoertuig.Beschrijving);
                commandText.Parameters.AddWithValue("@bouwjaar", idVoertuig.Bouwjaar);
                commandText.Parameters.AddWithValue("@model", idVoertuig.Model);
                commandText.Parameters.AddWithValue("@type", idVoertuig.Type);
                commandText.Parameters.AddWithValue("@merk", idVoertuig.Merk);

                commandText.Parameters.AddWithValue("@maxbelasting", (idVoertuig as GetrokkenVoertuig)?.Maxbelasting ?? (object)DBNull.Value);
                commandText.Parameters.AddWithValue("@afmetingen", (idVoertuig as GetrokkenVoertuig)?.Afmeting ?? (object)DBNull.Value);
                commandText.Parameters.AddWithValue("@transmissie", (idVoertuig as MotorVoertuig)?.Transmissie ?? (object)DBNull.Value);

                commandText.Parameters.AddWithValue("@brandstof", (idVoertuig as MotorVoertuig)?.Brandstof ?? (object)DBNull.Value);
                commandText.Parameters.AddWithValue("@gewicht", (idVoertuig as GetrokkenVoertuig)?.Gewicht ?? (object)DBNull.Value);
                commandText.Parameters.AddWithValue("@geremd", (idVoertuig as GetrokkenVoertuig)?.Geremd ?? (object)DBNull.Value);

                int createID = Convert.ToInt32(commandText.ExecuteScalar());
                return createID;
            }
        }
    }
}
