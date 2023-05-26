using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace MyClassLibrary
{
    public class Voertuig
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Beschrijving { get; set; }
        public int Bouwjaar { get; set; }
        public string Merk { get; set; }
        public string Model { get; set; }
        public int Type { get; set; }
        public Gebruiker Eigenaar_Id { get; set; }

        // Initialiseert een Voertuig-object met behulp van de waarden uit de database 
        public Voertuig(SqlDataReader rdr)
        {
            Id = Convert.ToInt32(rdr["Id"]);
            Name = Convert.ToString(rdr["Naam"]);
            Beschrijving = Convert.ToString(rdr["Beschrijving"]);
            Bouwjaar = Convert.ToInt32(rdr["Bouwjaar"]);
            Merk = Convert.ToString(rdr["Merk"]);
            Model = Convert.ToString(rdr["Model"]);
            Type = Convert.ToInt32(rdr["Type"]);
            Eigenaar_Id = Gebruiker.GetById(Convert.ToInt32(rdr["Eigenaar_Id"]));
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
    }
}
