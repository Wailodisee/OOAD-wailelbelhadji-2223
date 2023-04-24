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
        public List<Product> Producten { get; private set; }
        public Betaalwijze BetaaldMet { get; set; }
        public string Kassier { get; set; }
        public decimal Totaalprijs => BerekenTotaalprijs();

        public Ticket(string kassier, Betaalwijze betaaldMet = Betaalwijze.Cash)
        {
            Kassier = kassier;
            BetaaldMet = betaaldMet;
            Producten = new List<Product>();
        }

        public void VoegProductToe(Product product)
        {
            Producten.Add(product);
        }

        public void DrukTicket()
        {
            Console.WriteLine("**************** TICKET DELHAIZE ****************");
            Console.WriteLine($"Naam Kassier: {Kassier}");
            Console.WriteLine($"Betaald met: {BetaaldMet}");
            Console.WriteLine("Gekocht producten:");
            foreach (var product in Producten)
            {
                Console.WriteLine(product.errorString());
            }
            Console.WriteLine($"Totaalprijs: {Totaalprijs:C}");
            if (BetaaldMet == Betaalwijze.Visa)
            {
                Console.WriteLine("Kosten: €0,12");
            }
            Console.WriteLine("*******************************************");
        }

        private decimal BerekenTotaalprijs()
        {
            decimal totaal = Producten.Sum(product => product.Eenheidsprijs);
            if (BetaaldMet == Betaalwijze.Visa)
            {
                totaal += 0.12m;
            }
            return totaal;
        }
    }
}
