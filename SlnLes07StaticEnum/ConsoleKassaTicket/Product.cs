using System;

namespace ConsoleKassaTicket
{

    class Product
    {
        public string Naam { get; set; }
        public decimal Prijs { get; set; }
        
        // Constructor
        public Product(string naam, decimal prijs)
        {
            Naam = naam;
            Prijs = prijs;
        }
    }

    // Enumeratie voor betaalwijzen
    enum Betaalwijze
    {
        Visa, Cash, Bancontact 
    }

}
