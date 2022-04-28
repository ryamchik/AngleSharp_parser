namespace AngleSharp_parser
{
    class Program
    {
        static void Main(string[] args)
        {
            //System.Console.WriteLine("Начало парсинга...\n");

            var address = "https://www.toy.ru/catalog/boy_transport/";
            var parser = new Parser(address);
            var records = parser.Parse().Result;
            Document.Make(records);

            System.Console.WriteLine("Парсинг успешно произведен!\n");

            var region = "Ростов-на-Дону";
            System.Console.WriteLine("Смена региона на {0}", region);

            //parser.Change(region);

            //System.Console.WriteLine("Начало парсинга...\n");
            //var records = parser.Parse().Result;
            //Document.Make(records);

            System.Console.WriteLine("Парсинг успешно произведен!\n");
        }
    }
}
