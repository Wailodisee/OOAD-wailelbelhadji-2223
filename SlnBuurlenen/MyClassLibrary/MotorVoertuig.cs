namespace MyClassLibrary
{
    public enum TransmissieEnum
    {
        Manueel = 0,
        Automatisch = 1
    }
    public enum BrandstofEnum
    {
        Benzine = 0,
        Diesel = 1,
        LPG = 2
    }

    internal class MotorVoertuig : Voertuig
    {
        public TransmissieEnum? Transmissie { get; set; }
        public BrandstofEnum? Brandstof { get; set; }
    }
}
