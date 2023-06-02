using System;
using System.Data.SqlClient;

namespace MyClassLibrary
{
    public enum TransmissieEnum
    {
        Manueel = 1,
        Automatisch = 2
    }
    public enum BrandstofEnum
    {
        Benzine = 1,
        Diesel = 2,
        LPG = 3
    }

    public class MotorVoertuig : Voertuig
    {
        public TransmissieEnum? Transmissie { get; set; }
        public BrandstofEnum? Brandstof { get; set; }

        public MotorVoertuig()
        {
        }

        // MotorVoertuig-object wordt aangemaakt + Transmissie en Brandstof wordt geconfigureerd met juiste waarden
        public MotorVoertuig(SqlDataReader rdr)
            : base(rdr)
        {
         this.Transmissie = rdr.IsDBNull(rdr.GetOrdinal("Transmissie")) ? null : (TransmissieEnum?)Convert.ToInt32(rdr["Transmissie"]);
         this.Brandstof = rdr.IsDBNull(rdr.GetOrdinal("Brandstof")) ? null : (BrandstofEnum?)Convert.ToInt32(rdr["Brandstof"]);
        }
    }
}
