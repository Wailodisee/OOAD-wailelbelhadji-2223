namespace ConsoleKassaTicket
{
    internal class Product
    {
        public string Naam { get; set; }
        public decimal Eenheidsprijs { get; set; }
        public string Code { get; set; }

        public static bool ValideerCode(string code)
        {
            if (code.Length != 6 || !code.StartsWith("P"))
            {
                return false;
            }
            return int.TryParse(code.Substring(1), out _);
        }

        public string errorString()
        {
            return $"({Code}) {Naam} {Eenheidsprijs:C}";
        }
    }
}
