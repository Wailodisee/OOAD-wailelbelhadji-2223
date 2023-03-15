using System;
using System.IO;


namespace ConsoleCsv
{
    internal class Program
    {
        static void Main(string[] args)
        {
            {
                string[] spellen = { "schaak" , "dammen" , "backgammon" };

                string[] spelers = { "Zakaria" , "Saleha" , "Indra" , "Ralph" , "Francisco" , "Marie" };

                Random rdm = new Random();

                using (StreamWriter writer = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Wedstrijden.csv"))
                {
                    for (int i = 0; i < 100; i++)
                    {
                        string game = spellen[rdm.Next(0, spellen.Length)];

                        string speler1 = spelers[rdm.Next(0, spelers.Length)];

                        string speler2 = spelers[rdm.Next(0, spelers.Length)];

                        while (speler1 == speler2)
                        {
                            speler2 = spelers[rdm.Next(0, spelers.Length)];
                        }
                        int scores1 = rdm.Next(0, 4);

                        int scores2 = rdm.Next(0, 4);

                        while (scores1 + scores2 != 3)

                        {
                            scores2 = rdm.Next(0, 4);
                        }
                        writer.WriteLine($"{speler1};{speler2};{game};{scores1}-{scores2};");
                    }
                }
                Console.WriteLine("bestand 'wedstrijden.csv' werd opgeslagen op het bureaublad.");

                Console.ReadKey();
            }
        }
    }
}