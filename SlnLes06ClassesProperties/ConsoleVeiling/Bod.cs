namespace ConsoleVeiling
{
    internal class Bod
    {
        public Koper Koper { get; set; }
        public double Bedrag { get; set; }

        
        public Bod(Koper mijnKoper, float bedrag)
        {
            Koper = mijnKoper;

            Bedrag = bedrag;
        }
    }
}
