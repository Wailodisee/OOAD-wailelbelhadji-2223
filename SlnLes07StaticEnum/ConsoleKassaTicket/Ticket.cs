using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleKassaTicket.Program;

namespace ConsoleKassaTicket
{
    internal class Ticket
    {
        public Product[] Producten { get; set; }
        public Betaalwijze Betaalwijze { get; set; }
        public string KassierNaam { get; set; }

        // Totaalprijs als eigenschap
        public decimal TotaalPrijs
        {
            get
            {
                decimal totaal = 0;
                foreach (var product in Producten)
                {
                    totaal += product.Prijs;
                }
                return totaal;
            }
        }

        // Constructor
        public Ticket(Product[] producten, Betaalwijze betaalwijze, string kassierNaam)
        {
            Producten = producten;
            Betaalwijze = betaalwijze;
            KassierNaam = kassierNaam;
        }

        public Ticket(Product[] producten, Betaalwijze betaalwijze)
        {
            Producten = producten;
            Betaalwijze = betaalwijze;
        }

        // Methode om ticket af te drukken
        public void DrukTicket()
        {
            Console.WriteLine("KASSATICKET");
            Console.WriteLine("===========");
            Console.WriteLine($"Uw kassier: {KassierNaam}");
            Console.WriteLine("");
            foreach (var product in Producten)
            {
                Console.WriteLine($"{product.Naam}{product.Prijs}");
            }
            Console.WriteLine("--------------");
            Console.WriteLine($"Visa Kosten: 0,12");
            Console.WriteLine($"Totaal: {TotaalPrijs}");
        }

        // Methode om product te verwijderen
        public void VerwijderProduct(Product product)
        {
            List<Product> productList = Producten.ToList();
            productList.Remove(product);
            Producten = productList.ToArray();
        }
    }

}