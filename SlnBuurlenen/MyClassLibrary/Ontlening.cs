using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MyClassLibrary
{
    public enum OntleningStatus
    {
        InAanvraag = 1,
        Goedgekeurd = 2,
        Verworpen = 3
    }
    public class Ontlening
    {
        public int Id { get; set; }
        public DateTime Vanaf { get; set; }
        public DateTime Tot { get; set; }
        public string Bericht { get; set; }
        public OntleningStatus Status { get; set; }
        public int VoertuigId { get; set; }
        public int AanvragerId { get; set; }

        // Haalt waarden van de database + initialiseert Ontlening-object 
        public Ontlening(SqlDataReader rdr)
        {
            Id = (int)rdr["id"];

            Vanaf = (DateTime)rdr["vanaf"];

            Tot = (DateTime)rdr["tot"];

            Bericht = (string)rdr["bericht"];

            Status = (OntleningStatus)(byte)rdr["status"];

            VoertuigId = (int)rdr["voertuig_Id"];

            AanvragerId = (int)rdr["aanvrager_id"];
        }

        public Ontlening()
        {
            // Nieuwe ontleningen komen hier terecht
        }

        // Nieuwe Ontlening inserten in database
        public static void Insert(Ontlening mijnOntlening)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Ontlening (vanaf, tot, bericht, status, voertuig_id, aanvrager_id) " + "VALUES (@vanaf, @tot, @bericht, @status, @voertuig_id, @aanvrager_id)";

                SqlCommand newCommand = new SqlCommand(query, connection);
                newCommand.Parameters.AddWithValue("@vanaf", mijnOntlening.Vanaf);
                newCommand.Parameters.AddWithValue("@tot", mijnOntlening.Tot);
                newCommand.Parameters.AddWithValue("@bericht", mijnOntlening.Bericht);
                newCommand.Parameters.AddWithValue("@status", (int)mijnOntlening.Status);
                newCommand.Parameters.AddWithValue("@voertuig_id", mijnOntlening.VoertuigId);
                newCommand.Parameters.AddWithValue("@aanvrager_id", mijnOntlening.AanvragerId);

                newCommand.ExecuteNonQuery();
            }
        }

        // Nieuwe Ontlening verwijderen in database
        public static void Delete(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Ontlening WHERE id=@id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                command.ExecuteNonQuery();
            }
        }

        // Nieuwe Ontlening updaten in database
        public static void Update(Ontlening mijnOntlening)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE Ontlening SET vanaf=@vanaf, tot=@tot, bericht=@bericht, status=@status, voertuig_id=@voertuig_id, aanvrager_id=@aanvrager_id WHERE id=@id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@vanaf", SqlDbType.DateTime).Value = mijnOntlening.Vanaf;
                command.Parameters.Add("@tot", SqlDbType.DateTime).Value = mijnOntlening.Tot;
                command.Parameters.Add("@bericht", SqlDbType.VarChar).Value = mijnOntlening.Bericht;
                command.Parameters.Add("@status", SqlDbType.TinyInt).Value = (byte)mijnOntlening.Status;
                command.Parameters.Add("@voertuig_id", SqlDbType.Int).Value = mijnOntlening.VoertuigId;
                command.Parameters.Add("@aanvrager_id", SqlDbType.Int).Value = mijnOntlening.AanvragerId;
                command.Parameters.Add("@id", SqlDbType.Int).Value = mijnOntlening.Id;

                command.ExecuteNonQuery();
            }
        }

        // Haalt alle ontleningen op uit de database + retourneert deze in een lijst van Ontlening-objecten.
        public static List<Ontlening> GetAll(int idGebruiker)
        {
            List<Ontlening> mijnOntleningen = new List<Ontlening>();

            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Ontlening WHERE aanvrager_id = @gebruikerId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@gebruikerId", SqlDbType.Int).Value = idGebruiker;

                SqlDataReader rdrr = command.ExecuteReader();

                while (rdrr.Read())
                {
                    Ontlening mijnOntlening = new Ontlening();
                    mijnOntlening.Id = (int)rdrr["id"];
                    mijnOntlening.Vanaf = (DateTime)rdrr["vanaf"];
                    mijnOntlening.Tot = (DateTime)rdrr["tot"];
                    mijnOntlening.Bericht = (string)rdrr["bericht"];
                    mijnOntlening.Status = (OntleningStatus)(byte)rdrr["status"];
                    mijnOntlening.VoertuigId = (int)rdrr["voertuig_id"];
                    mijnOntlening.AanvragerId = (int)rdrr["aanvrager_id"];

                    mijnOntleningen.Add(mijnOntlening);
                }
            }

            return mijnOntleningen;
        }

        // Haalt ontleningen uit de database waarvan de voertuig_id = Id's +  retourneert deze in een lijst van Ontlening-objecten.
        public static List<Ontlening> CatchOntleningenByVoertuigId(int idEigenaar)
        {
            List<Ontlening> mijnOntleningen = new List<Ontlening>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Ontlening WHERE voertuig_id IN (SELECT Id FROM Voertuig WHERE Eigenaar_Id = @eigenaarId)", connection);
                command.Parameters.AddWithValue("@eigenaarId", idEigenaar);
                SqlDataReader rdrr = command.ExecuteReader();
                while (rdrr.Read())
                {
                    Ontlening ontlening = new Ontlening(rdrr);
                    mijnOntleningen.Add(ontlening);
                }
            }
            return mijnOntleningen;
        }
    }
}
