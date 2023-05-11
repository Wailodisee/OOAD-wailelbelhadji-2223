using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public class Auto
    {
        public string model { get; set; }

        public static List<Auto> GetAllAutos()
        {
            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            //SQL in WPF
        }

    }
}
