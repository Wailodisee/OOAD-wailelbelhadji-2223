using System;
using System.Windows.Controls;

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

        public string GenerateVcardCode()
        {
            string content = "BEGIN:VCARD" + Environment.NewLine;
            content += "VERSION:3.0" + Environment.NewLine;

            if (voornaam != null && achternaam != null)
            {
                content += $"FN;CHARSET=UTF-8:{voornaam} {achternaam}" + Environment.NewLine;
                content += $"N;CHARSET=UTF-8:{voornaam};{achternaam};;;" + Environment.NewLine;
            }

            if (gender != default(char))
            {
                content += $"GENDER:{gender}" + Environment.NewLine;
            }

            if (DatumPicker != null)
            {
                content += $"BDAY:{DatumPicker}" + Environment.NewLine;
            }

            if (priveEmail != null)
            {
                content += $"EMAIL;CHARSET=UTF-8;type=HOME,INTERNET:{priveEmail}" + Environment.NewLine;
            }

            if (priveTelefoon != null)
            {
                content += $"TEL;TYPE=WORK,VOICE:{priveTelefoon}" + Environment.NewLine;
            }

            if (bedrijf != null)
            {
                content += $"ORG;CHARSET=UTF-8:{bedrijf}" + Environment.NewLine;
            }

            if (JobTitel != null)
            {
                content += $"TITLE;CHARSET=UTF-8:{JobTitel}" + Environment.NewLine;
            }

            if (WerkEmail != null)
            {
                content += $"EMAIL;CHARSET=UTF-8;type=WORK,INTERNET:{WerkEmail}" + Environment.NewLine;
            }

            if (WerkTelefoon != null)
            {
                content += $"TEL;TYPE=WORK,VOICE:{WerkTelefoon}" + Environment.NewLine;
            }

            content += "END:VCARD";

            return content;
        }

        // Lege constructor 

        public Vcard()
        {

        }

        public Vcard(string voornaamV, string achternaamV, DateTime datumV, char genderV, string privemailV, string privetelefoonV, string bedrijfV, string jobitelV, string werkmailV, string werktelV)
        {
            voornaam = voornaamV;
            achternaam = achternaamV;
            DatumPicker = datumV;
            gender = genderV;
            priveEmail = privemailV;
            priveTelefoon = privetelefoonV;
            bedrijf = bedrijfV;
            JobTitel = jobitelV;
            WerkEmail = werkmailV;
            WerkTelefoon = werktelV;
        }
    }
}

