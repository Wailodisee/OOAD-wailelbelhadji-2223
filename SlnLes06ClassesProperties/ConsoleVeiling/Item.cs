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

        // constructor
        public Item() { }
        public Item(string naam, int minPrijs)
        {
            Naam = naam;
            lastPrijs = false;
            Verkocht = false;
            EersteKoper = null;
            AllBod = new List<Bod>();
        }

        public void NieuweBod(Bod mijnBoden)
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

        private Koper WinnerBod()
        {
            if (AllBod.Count == 0)
            {
                Console.WriteLine("Er zijn geen biedingen gedaan.");
            }
            Bod prijs = AllBod[0];
            foreach (Bod b in AllBod)
            {
                if (prijs.Bedrag < b.Bedrag)
                {
                    prijs = b;
                }
            }
            return prijs.Koper;
        }
        public void GeslotenVeiling()
        {
            if (lastPrijs)
            {
                Console.WriteLine("Error: De veiling is nu toe.");
            }
            lastPrijs = true;
            EersteKoper = WinnerBod();
            EersteKoper.NewItem(this);
            Verkocht = true;
        }
    }
    }
