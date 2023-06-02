using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public class Foto
    {
        public int Id { get; set; }
        public byte[] Data { get; set; }
        public int VoertuigId { get; set; }

        // Leest gegevens van de database en return een Foto-object
        private static Foto RdrPicture(SqlDataReader rdr)
        {
            return new Foto
            {
                Data = (byte[])rdr["Data"],

                Id = (int)rdr["Id"],

                VoertuigId = (int)rdr["Voertuig_id"]
            };
        }

        // Haalt elk foto van elk voertuig en return ze in een lijst
        public static List<Foto> GetAllAutoPictures(int idVoertuig)
        {
            List<Foto> pictures = new List<Foto>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM Foto WHERE Voertuig_id=@voertuigId", connection);

                command.Parameters.AddWithValue("@voertuigId", idVoertuig);

                SqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    Foto picture = RdrPicture(rdr);

                    pictures.Add(picture);
                }
            }
            return pictures;
        }

        // Delete alle rijen uit de tabel foto
        public static void DeleteById(int voertuigId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                string commandText = "DELETE FROM Foto WHERE Voertuig_id=@voertuigId";

                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@voertuigId", voertuigId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        // haalt een lijst Foto-objecten op uit de database
        public static List<Foto> GetAllPictureId(int idVoertuig)
        {
            List<Foto> listPictures = new List<Foto>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM Foto WHERE Voertuig_id=@voertuigId", connection);

                command.Parameters.AddWithValue("@voertuigId", idVoertuig);

                SqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    Foto picture = RdrPicture(rdr);
                    listPictures.Add(picture);
                }
            }
            return listPictures;
        }

        // Voegt een nieuwe rij toe aan de tabel Foto in de database
        public void GetImagesFromDB()
        {
            string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            string commandText = "INSERT INTO Foto (Data, Voertuig_id) VALUES (@Data, @voertuigId)";

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@Data", this.Data);
                    command.Parameters.AddWithValue("@voertuigId", this.VoertuigId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
