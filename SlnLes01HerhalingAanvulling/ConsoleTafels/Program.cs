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
            DrukTafel (4, 8);
            DrukTafel (2, 5);
            VraagPositiefGetal();
            Console.ReadLine();
        }

        public static void DrukTafel(int cijfer, int lengte)
        {
            Console.WriteLine(lengte + " x " + cijfer + " tafel:");

            for (int i = 1; i <= lengte; i++)
            {
                int terugCijfer = cijfer * i;
                Console.WriteLine(cijfer + " x " + i + " = " + terugCijfer);
            }
            Console.WriteLine();
        }

        public static void VraagPositiefGetal()
        {
            Console.Write(" Geef een getal : ");
            int cijfer = Convert.ToInt32(Console.ReadLine());

            if (cijfer <= 0)
            {
                Console.Write(" Het getal moet positief zijn ! ");

                VraagPositiefGetal();
            }
            else
            {
                Console.Write(" geef de lengte : ");

                int lengte = Convert.ToInt32(Console.ReadLine());

                DrukTafel(cijfer, lengte);
            }
            Console.ReadLine();
        }
    }
}
