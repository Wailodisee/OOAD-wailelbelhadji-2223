using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    internal class GetrokkenVoertuig : Voertuig
    {
        public int? Gewicht { get; set; }
        public int? Maxbelasting { get; set; }
        public int Afmeting { get; set; }
        public bool? Geremd { get; set; }
    }
}
