using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVeiling
{
    internal class Item
    {
        public List<Bod> AllBod { get; set; }
        public Koper EersteKoper { get; set; }
        public string Naam { get; set; }
        public double minPrijs { get; set; }
        public bool lastPrijs { get; set; }
        public bool Verkocht { get; set; }

        // constructor */
        
        public Item(string naam, int minPrijs)
        {
            Naam = naam;

            lastPrijs = false;

            Verkocht = false;

            EersteKoper = null;

            AllBod = new List<Bod>();
        }
        /*Nieuwe Bod maken*/

        public void ToevoegenBod(Bod mijnBoden)
        {
            if (lastPrijs)
            {
                Console.WriteLine("De veiling werd net afgesloten :(");
            }
            if (mijnBoden.Bedrag < minPrijs)
            {
                Console.WriteLine("minimumprijs werd niet gerespecteerd.");
            }
            AllBod.Add(mijnBoden);
        }

        /*methode Winner van het bod*/

        private Koper WinnerBod()
        {
            if (AllBod.Count == 0)
            {
                Console.WriteLine("Er zijn geen biedingen gedaan.");
            }
            Bod Prijs = AllBod[0];
            foreach (Bod boden in AllBod)
            {
                if (Prijs.Bedrag < boden.Bedrag)
                {
                    Prijs = boden;
                }
            }
            return Prijs.Koper;
        }

        /*Veiling afsluiten*/

        public void VeilingToe()
        {
            if (lastPrijs)
            {
                Console.WriteLine("De veiling werd net afgesloten :(");
            }
            lastPrijs = true;

            EersteKoper = WinnerBod();

            EersteKoper.NewItem(this);

            Verkocht = true;
        }
    }
}
