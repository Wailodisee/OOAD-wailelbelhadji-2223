using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVeiling
{
    internal class Koper
    {
        public string mijnNaam { get; set; }

        public List<Item> BodItem { get; set; }

        // constructor aanameken */
        public Koper(string Naam)
        {
            mijnNaam = Naam;

            BodItem = new List<Item>();
        }
        /* Item toevoegen */
        public void NewItem(Item Items)
        {
            BodItem.Add(Items);
        }
    }
}
