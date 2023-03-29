using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfVcardEditor
{
    internal class Vcard
    {
        public string voornaam { get; set; }
        public string achternaam { get; set; }
        public DateTime DatumPicker { get; set; }
        public char gender { get; set; }
        public string priveEmail { get; set; }
        public string priveTelefoon { get; set; }
        public string bedrijf { get; set; }
        public string JobTitel { get; set; }
        public string WerkEmail { get; set; }
        public string WerkTelefoon { get; set; }
    }
}
