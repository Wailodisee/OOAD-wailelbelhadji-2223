using System;

namespace ConsoleKassaTicket
{

    class Product
    {
        public string Naam { get; set; }
        public decimal EenheidsPrijs { get; set; }
        
        // Constructor
        public Product(string naam, decimal prijs)
        {
            Naam = naam;
            EenheidsPrijs = prijs;
        }
        public static bool ValideerCode(string code)
        {
            // Controleer of de code minimaal 7 karakters lang is
            if (code.Length != 7)
            {
                return false;
            }

            // Controleer of de code start met "P"
            if (!code.StartsWith("P"))
            {
                return false;
            }

            // Controleer of de overige karakters cijfers zijn
            string cijfers = code.Substring(1);
            foreach (char c in cijfers)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;
        }
        public override string ToString()
        {
          return $"{Naam} {EenheidsPrijs.ToString("F2")}";
        }
    }

    // Enumeratie voor betaalwijzen
    enum Betaalwijze
    {
        Visa = 0, Cash = 1, Bancontact = 2 
    }

}
