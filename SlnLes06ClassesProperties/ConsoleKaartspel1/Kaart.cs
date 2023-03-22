using System;

namespace ConsoleKaartspel1
{
    class Kaart
    {
        private int _mijnNummer; //getal oproepen tussen 1-13

        private string _mijnWaarde; //waarde van kaarten

        //constructor voor nummer en kleur
        public Kaart(int hetNummer, string hetKleur)
        {
            Nummer = hetNummer;

            Kleur = hetKleur;
        }

        public int Nummer
        {
            get { return _mijnNummer; } //get set structuur
            set
            {
                if (value < 1 || value > 13)
                {
                    throw new ArgumentOutOfRangeException("Kies nu een cijfer tussen 1 en 13");
                }
                else
                {
                    _mijnNummer = value;
                }
            }
        }


        public string Kleur
        {
            get { return _mijnWaarde; } //get set structuur
            set
            {
                int correctAntwoorden = 0;

                string[] mijnList = new string[] { "C", "S", "H", "D" };

                for (int i = 0; i < mijnList.Length; i++)
                {
                    correctAntwoorden += mijnList[i] == value ? 1 : 0;
                }

                if (correctAntwoorden == 0)
                {
                    throw new ArgumentOutOfRangeException("Kies nu een waarde tussen : c, s, h of d");
                }
                else
                {
                    _mijnWaarde = value;
                }
            }
        }
    }
}
