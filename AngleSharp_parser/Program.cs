using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;

namespace AngleSharp_parser
{
    class Program
    {
        static void Main(string[] args)
        {
            var address = "https://www.toy.ru/catalog/toys-spetstekhnika/childs_play_lvy025_fermerskiy_traktor/";
            //var address = "https://www.toy.ru/catalog/toys-spetstekhnika/childs_play_lvy023_pozharnaya_mashina/";
            var parser = new Parser(address);
            var result = parser.Parse().Result;

            Console.WriteLine(result.Region);

            Console.WriteLine(new string('=', 20));

            //foreach (var breadcrumb in result.BreadCrumbs)
            Console.WriteLine(result.BreadCrumbs);

            Console.WriteLine(new string('=', 20));

            Console.WriteLine(result.ProductName);

            Console.WriteLine(new string('=', 20));

            Console.WriteLine(result.PriceCurrent);

            Console.WriteLine(new string('=', 20));

            Console.WriteLine(result.PriceOld);

            Console.WriteLine(new string('=', 20));

            Console.WriteLine(result.Available);

            Console.WriteLine(new string('=', 20));

            foreach (var item in result.Images)
                Console.WriteLine(item);

            Console.WriteLine(new string('=', 20));

            Console.WriteLine(result.Link);

            // Запись результата парсинга в csv.

            // Доп.плюшки с регионом.
        }
    }

    class Parser
    {
        string address = string.Empty;

        public Parser(string address)
        {
            this.address = address;
        }

        public async Task<ProductInfo> Parse()
        {
            var config = Configuration.Default.WithDefaultLoader();
            var document = await BrowsingContext.New(config).OpenAsync(address);
            var result = new ProductInfo();

            var cellSelector = ".select-city-link";
            var cell = document.QuerySelector(cellSelector).QuerySelector("a");
            result.Region = cell.TextContent.Trim();

            cellSelector = ".breadcrumb-item";
            var breadcrumbs = document.QuerySelectorAll(cellSelector);
            var ar = new List<string>();

            foreach (var breadcrumb in breadcrumbs)
            {
                ar.Add(breadcrumb.TextContent);
            }

            result.BreadCrumbs = String.Join(",", ar.ToArray());

            result.ProductName = GetArgument(".detail-name", document);
            result.PriceCurrent = GetArgument(".price", document);
            result.PriceOld = GetArgument(".old-price", document);
            result.Available = GetArgument(".ok", document, "Нет в наличии");
            
            foreach (var image in document.Images)
            {
                result.Images.Add(image.Source);
            }

            result.Link = document.Url;

            return result;
        }

        public string GetArgument(string cellselector, AngleSharp.Dom.IDocument document, string text = "")
        {
            var cell = document.QuerySelector(cellselector);
            var result = (cell != null) ? cell.TextContent.Trim() : text;

            return result;
        }
    }



    public class ProductInfo
    {
        public string Region { get; set; }
        public string ProductName { get; set; }
        public string BreadCrumbs { get; set; }
        public string PriceCurrent { get; set; }
        public string PriceOld { get; set; }
        public string Available { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public string Link { get; set; }
    }
}
