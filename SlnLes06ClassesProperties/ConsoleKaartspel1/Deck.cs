using System;
using System.Collections.Generic;

namespace ConsoleKaartspel1
{
    class Deck
    {
        static Random random2 = new Random();
        const int HOEVEELHEID_KAARTEN = 52;
        public List<Kaart> Kaart { get; set; } = new List<Kaart>(); //lijst declareren kaarten

        //constructor maken
        public Deck()
        {
            for (int i = 1; i < 13; i++) //for om kaarten te vullen
            {
                Kaart.Add(new Kaart(i, "C")); //kaart D toevoegen

                Kaart.Add(new Kaart(i, "S")); //kaart H toevoegen

                Kaart.Add(new Kaart(i, "H")); //kaart C toevoegen

                Kaart.Add(new Kaart(i, "D")); //kaart S toevoegen
            }
        }

        public void Schudden() //methode schudden
        {
            int ix;
            for (int tijdix = 0; tijdix < Kaart.Count; tijdix++) //for om kaarten te vullen
            {
                ix = random2.Next(0, Kaart.Count);

                Kaart kaart = Kaart[tijdix];

                Kaart[tijdix] = Kaart[ix];

                Kaart[ix] = kaart;
            }

            for (int i = 0; i < Kaart.Count; i++)
            {
                ix = random2.Next(0, Kaart.Count);

                Kaart[i] = Kaart[ix];
            }
        }
        public Kaart NeemKaart() //methode die kaart teruggeeft
        {
            Kaart genomenKaart = Kaart[0];

            Kaart.RemoveAt(0);

            return genomenKaart;
        }

        public int NogKaarten //methode indien er een errror is met resterende kaarten
        {
            get
            {
                return Kaart.Count;
            }
        }
    }
}
