using System;
using System.Collections.Generic;

namespace ConsoleKaartspel1
{
    class Speler
    {
        static Random random1 = new Random(); //random declareren
        public string Naam { get; set; }
        public List<Kaart> Kaarten { get; set; } = new List<Kaart>(); //lijst declareren kaarten
        public bool HeeftNogKaarten
        {
            get { return Kaarten.Count > 0; }
        }

        //CONSTRUCTOR NAMEN
        public Speler(string mijnNaam) //methode spelers
        {
            Naam = mijnNaam;
        }

        //CONSTRUCTOR NAMEN + KAARTEN
        public Speler(string Namen, List<Kaart> mijnKaarten) //lijst declareren namen 
        {
            Naam = Namen;

            Kaarten = mijnKaarten;
        }

        public Kaart LegKaart() //methode kaart leggen 
        {
            Kaart gegevenKaart = Kaarten[random1.Next(0, Kaarten.Count)];

            Kaarten.Remove(gegevenKaart);

            return gegevenKaart;
        }
    }
}
