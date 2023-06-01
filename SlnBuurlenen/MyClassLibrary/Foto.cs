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
        public static List<Foto> GetAllAutoPictures(int voertuigId)
        {
            List<Foto> pictures = new List<Foto>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM Foto WHERE Voertuig_id=@voertuigId", connection);

                command.Parameters.AddWithValue("@voertuigId", voertuigId);

                SqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    Foto picture = RdrPicture(rdr);

                    pictures.Add(picture);
                }
            }
            return pictures;
        }
    }
}
