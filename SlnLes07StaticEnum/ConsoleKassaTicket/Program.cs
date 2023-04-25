using System;
using System.Collections.Generic;

namespace ConsoleKassaTicket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Producten aanmaken
            Product product1 = new Product("(P02384) bananen: ", 1.75m);
            Product product2 = new Product("(P01820) brood: ", 2.10m);
            Product product3 = new Product("(P02384) kaas: ", 3.99m);
            Product product4 = new Product("(P98754) koffie : ", 4.10m);

            // Ticket aanmaken
            Product[] producten = new Product[] { product1, product2, product3, product4 };
            Ticket ticket = new Ticket(producten, Betaalwijze.Cash, "Wail");

            // Ticket afdrukken
            ticket.DrukTicket();

            Console.ReadLine();
        }
    }
}
