﻿using System;
using System.Linq;
using System.Threading;

namespace ConsoleVeiling
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Koper eersteKoper = new Koper("wail");

            Koper tweedeKoper = new Koper("anas");

            Koper derdeKoper = new Koper("souli");

            Item Komputer = new Item("Komputer HP", 800);

            Item Auto = new Item("Auto mercededs", 20000);

            /*try catch declareren*/

            try
            {
                Komputer.ToevoegenBod(new Bod(eersteKoper, 880));

                Komputer.ToevoegenBod(new Bod(tweedeKoper, 900));

                Komputer.ToevoegenBod(new Bod(derdeKoper, 910));


                Console.WriteLine();

                Console.WriteLine($"biedingen zijn klaar voor dit item:  {Komputer.Naam}");

                foreach (var mijnBod in Komputer.AllBod)
                {
                    Console.WriteLine($"{mijnBod.Koper.mijnNaam} heeft een totaal van  {mijnBod.Bedrag} euro geboden.");
                }

                Console.WriteLine("binnen 30 seconden is het veiling klaar.");

                /*timer declareren*/

                Thread.Sleep(30000);

                Komputer.VeilingToe();

                Bod HoogsteBod = Komputer.AllBod.OrderByDescending(boden => boden.Bedrag).FirstOrDefault();

                if (HoogsteBod != null)
                {
                    Console.WriteLine($"Het item: {Komputer.Naam} word gewonnen door: {HoogsteBod.Koper.mijnNaam} met een prijs van {HoogsteBod.Bedrag} euro.");
                }
                else
                {
                    Console.WriteLine($"Geen biedingen mogelijk voor dit item:{Komputer.Naam}.");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Een fout is opgetreden");
            }
            /*try catch declareren*/

            try
            {
                Auto.ToevoegenBod(new Bod(eersteKoper, 25000));

                Auto.ToevoegenBod(new Bod(tweedeKoper, 26000));

                Auto.ToevoegenBod(new Bod(derdeKoper, 30000));

                Console.WriteLine();

                Console.WriteLine($"Geen biedingen mogelijk voor dit item:{Auto.Naam}");

                foreach (var boden in Auto.AllBod)
                {
                    Console.WriteLine($"{boden.Koper.mijnNaam} heeft een totaal van {boden.Bedrag} geboden.");
                }

                /*tweede timer declareren*/

                Console.WriteLine("binnen 30 seconden is het veiling klaar.");

                Thread.Sleep(30000);

                Auto.VeilingToe();

                Console.WriteLine($"Het item:{Auto.Naam} word gewonnen door: {Auto.EersteKoper.mijnNaam} met een prijs van: {Auto.AllBod[Auto.AllBod.Count - 1].Bedrag} euro.");
            }

            /*catch declareren*/

            catch (Exception )
            {
                Console.WriteLine("Een fout is opgetreden");
            }
            Console.WriteLine();

            ToonItems(Komputer);

            Console.WriteLine();

            ToonItems(Auto);

            Console.WriteLine();

            ToonWinnaars(tweedeKoper);

            ToonWinnaars(eersteKoper);

            ToonWinnaars(derdeKoper);

            Console.ReadLine();
        }

        static void ToonItems(Item NewItem)
        {
            if (NewItem.Verkocht)
            {
                Console.WriteLine($"Het item: -{NewItem.Naam}- werd gewonnen door: {NewItem.EersteKoper.mijnNaam} voor {NewItem.AllBod[NewItem.AllBod.Count - 1].Bedrag} euro.");
            }
            else
            {
                Console.WriteLine($"Het item: *{NewItem.Naam}* heeft geen koper.");
            }
        }

        static void ToonWinnaars(Koper mijnKoper)
        {
            if (mijnKoper.BodItem.Count == 0)
            {
                Console.WriteLine($"{mijnKoper.mijnNaam} bevat geen items.");
                return;
            }

            Console.WriteLine($"{mijnKoper.mijnNaam} bevat de volgende items:");

            for (int i = 0; i < mijnKoper.BodItem.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {mijnKoper.BodItem[i].Naam}");
            }
            Console.WriteLine();
        }
    }
}