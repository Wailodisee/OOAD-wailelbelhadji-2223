using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTafels
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string eerstedruktafel = DrukTafel(4, 8);
            Console.WriteLine(eerstedruktafel);

            string tweededruktafel = DrukTafel(2, 5);
            Console.WriteLine(tweededruktafel);

            Console.Write("Geef een getal :  ");
            string eerstegetal = Console.ReadLine();
            int tweedegetal = VraagPositiefGetal(eerstegetal);

            Console.Write("Geef nu een lengte :  ");
            string eerstelengte = Console.ReadLine();
            int tweedelengte = VraagPositiefGetal(eerstelengte);

            string derdetafel = DrukTafel(tweedegetal, tweedelengte);
            Console.WriteLine(derdetafel);

            Console.ReadLine();
        }

        private static string DrukTafel(int cijfer, int afmeting)
        {
            string woord = $"{cijfer}x{afmeting} tafel: ";
            woord += Environment.NewLine;

            for (int i = 1; i <= afmeting; i++)
            {
                int uitkomst = cijfer * i;
                woord += $"{cijfer} x {i} = {uitkomst}";
                woord += Environment.NewLine;
            }

            return woord;
        }

        private static int VraagPositiefGetal(string eerstewaarde)
        {
            int tweedewaarde = Convert.ToInt32(eerstewaarde);
            if (tweedewaarde > 0)
            {
                return tweedewaarde;
            }
            else
            {
                while (tweedewaarde < 0)
                {
                    Console.Write("Het getal moet positief zijn! Geef een getal: ");
                    eerstewaarde = Console.ReadLine();
                    tweedewaarde = Convert.ToInt32(eerstewaarde);
                }
                return tweedewaarde;
            }
            Console.ReadLine();
        }    
    }
}
