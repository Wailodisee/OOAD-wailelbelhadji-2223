using System;
namespace MyClassLibrary
{
    public enum OntleningStatus
    {
        InAanvraag = 1,
        Goedgekeurd = 2,
        Verworpen = 3
    }
    public class Ontlening
    {
        public int Id { get; set; }
        public DateTime Vanaf { get; set; }
        public DateTime Tot { get; set; }
        public string Bericht { get; set; }
        public OntleningStatus Status { get; set; }
        public int VoertuigId { get; set; }
        public int AanvragerId { get; set; }
    }
}
