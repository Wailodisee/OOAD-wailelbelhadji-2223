using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public class GetrokkenVoertuig : Voertuig
    {
        public int? Gewicht { get; set; }
        public int? Maxbelasting { get; set; }
        public string Afmeting { get; set; }
        public bool? Geremd { get; set; }

        // Initialiseert een object van de GetrokkenVoertuig-klasse + NULL-waarden worden gecontroleerd
        public GetrokkenVoertuig(SqlDataReader rdr)
                                : base(rdr)
        {    
            this.Afmeting = rdr["Afmetingen"].ToString(); 

            this.Maxbelasting = rdr.IsDBNull(rdr.GetOrdinal("Maxbelasting")) ? null : (int?)rdr["Maxbelasting"];

            this.Geremd = rdr.IsDBNull(rdr.GetOrdinal("Geremd")) ? null : (bool?)rdr["Geremd"];

            this.Gewicht = rdr.IsDBNull(rdr.GetOrdinal("Gewicht")) ? null : (int?)rdr["Gewicht"];
        }
    }
}
