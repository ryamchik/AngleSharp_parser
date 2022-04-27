

namespace AngleSharp_parser
{
    class Program
    {
        static void Main(string[] args)
        {
            //var address = "https://www.toy.ru/catalog/toys-spetstekhnika/childs_play_lvy025_fermerskiy_traktor/";
            //var address = "https://www.toy.ru/catalog/toys-spetstekhnika/childs_play_lvy023_pozharnaya_mashina/";
            var address = "https://www.toy.ru/catalog/boy_transport/";
            var parser = new Parser(address);
            parser.Parse();

            Document.Create();
            
            //var record = parser.ParseSite().Result;
            
            // Доп.плюшки с регионом.
        }
    }
}
