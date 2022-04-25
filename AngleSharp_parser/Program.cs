using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

            cellSelector = ".breadcrumb";
            var breadcrumbs = document.QuerySelector(cellSelector).QuerySelectorAll("a.breadcrumb-item");
            var buffer = new List<string>();

            foreach (var breadcrumb in breadcrumbs)
            {
                buffer.Add(breadcrumb.TextContent.Trim());
            }

            result.ProductName = GetArgument(".detail-name", document);
            buffer.Add(result.ProductName);
            result.BreadCrumbs = String.Join(",", buffer.ToArray());
            result.PriceCurrent = GetArgument(".price", document);
            result.PriceOld = GetArgument(".old-price", document);
            result.Available = GetArgument(".ok", document, "Нет в наличии");
            var images = new List<string>();

            foreach (var image in document.Images)
            {
                if (image.GetAttribute("class") == "img-fluid")
                     images.Add(image.Source.Trim());
            }

            result.Images = String.Join(",", images.ToArray());
            result.Link = document.Url;

            using (var stream = new StreamWriter("test.csv", false, Encoding.UTF8))
            {
                stream.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7}",
                    result.Region, result.ProductName, result.BreadCrumbs, result.PriceCurrent, 
                    result.PriceOld, result.Available, result.Images, result.Link);
            }

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
        public string Images { get; set; }
        public string Link { get; set; }
    }
}
