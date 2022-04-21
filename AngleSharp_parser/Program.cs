using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp;

namespace AngleSharp_parser
{
    class Program
    {
        static void Main(string[] args)
        {
            var address = "https://www.toy.ru/catalog/boy_transport/";
            var parser = new Parser(address);
            var result = parser.Parse();

            // Запись результата парсинга в csv.

            // Доп.плюшки с регионом.
        }
    }

    class Parser
    {
        string address = string.Empty;
        var config = Configuration.Default.WithDefaultLoader();

        public Parser(string address)
        {
            this.address = address;
        }

        public async Task<ProductInfo> Parse()
        {
            var document = await BrowsingContext.New(config).OpenAsync(address);
            var result = new ProductInfo();

            foreach (var image in document.Images)
            {
                result.Images.Add(image.Source);
            }

            return result;
        }
    }

    public class ProductInfo
    {
        public string Region { get; set; }
        public string ProductName { get; set; }
        public List<string> BreadCrumbs { get; set; } = new List<string>();
        public string PriceCurrent { get; set; }
        public string PriceOld { get; set; }
        public string Available { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public string Link { get; set; }
    }
}
