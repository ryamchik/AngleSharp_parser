namespace AngleSharp_parser
{
    class Program
    {
        static void Main(string[] args)
        {
            var address = "https://www.toy.ru/catalog/boy_transport/";
            var parser = new Parser(address);
            var records = parser.Parse().Result;

            Document.Make(records);

            // Доп.плюшки с регионом.
        }
    }
}
