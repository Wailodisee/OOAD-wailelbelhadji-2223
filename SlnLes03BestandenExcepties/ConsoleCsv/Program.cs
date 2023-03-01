using System;
using System.IO;


namespace ConsoleCsv
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Arrays met spelers en verschillende spelletjes. 
            string[] spelletjes = { "backgammon", "dammen", "schaak" };

            string[] deelnemers = { "Zakaria", "Saleha", "Indra", "Ralph", "Francisco", "Marie" };

            int GenereerScores = 100;

            // Genereer 100 scores
            Random rdm = new Random();

            string csvFile = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\wedstrijden.csv";

            using (StreamWriter write = new StreamWriter(csvFile))
            {
                for (int i = 0; i < GenereerScores; i++)
                {
                    // Selectionneer random spelers en random 
                    string player1 = deelnemers[rdm.Next(deelnemers.Length)];

                    string player2 = deelnemers[rdm.Next(deelnemers.Length)];

                    while (player2 == player1)
                    {
                        player2 = deelnemers[rdm.Next(deelnemers.Length)];
                    }

                    Console.ReadKey();

                    string spel = spelletjes[rdm.Next(spelletjes.Length)];

                    int scores1 = rdm.Next(3);

                    int scores2 = 2 - scores1;

                    write.WriteLine($"{i + 1};{player1};{player2};{spel};{scores1}-{scores2};");
                }
            }
            // Weergeven van de writeLine
            Console.WriteLine($"Weggeschreven naar {csvFile}");
            Console.ReadKey();
        }
    }
}
