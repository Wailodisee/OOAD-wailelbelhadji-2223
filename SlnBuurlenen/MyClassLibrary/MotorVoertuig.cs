using System;
using System.Data.SqlClient;

namespace MyClassLibrary
{
    public enum TransmissieEnum
    {
        Manueel = 0,
        Automatisch = 1
    }
    public enum BrandstofEnum
    {
        Benzine = 0,
        Diesel = 1,
        LPG = 2
    }

    public class MotorVoertuig : Voertuig
    {
        public TransmissieEnum? Transmissie { get; set; }
        public BrandstofEnum? Brandstof { get; set; }

        // MotorVoertuig-object wordt aangemaakt + Transmissie en Brandstof wordt geconfigureerd met juiste waarden
        public MotorVoertuig(SqlDataReader rdr)
            : base(rdr)
        {
         this.Transmissie = rdr.IsDBNull(rdr.GetOrdinal("Transmissie")) ? null : (TransmissieEnum?)Convert.ToInt32(rdr["Transmissie"]);
         this.Brandstof = rdr.IsDBNull(rdr.GetOrdinal("Brandstof")) ? null : (BrandstofEnum?)Convert.ToInt32(rdr["Brandstof"]);
        }
    }
}
