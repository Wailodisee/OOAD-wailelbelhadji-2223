using System;

namespace MyClassLibrary
{
    public enum GeslachtsEnum : byte
    {
        Man = 0,
        Vrouw = 1
    }
    public class Gebruiker
    {
        public int Id { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string Email { get; set; }
        public string Paswoord { get; set; }
        public DateTime Aanmaakdatum { get; set; }
        public byte[] Profielfoto { get; set; }
        public GeslachtsEnum Geslacht { get; set; }
    }
}
