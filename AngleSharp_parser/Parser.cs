using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AngleSharp;

namespace AngleSharp_parser
{
    public class Parser
    {
        string address = string.Empty;
        AngleSharp.Dom.IDocument document;

        public Parser(string address)
        {
            this.address = address;
        }

        public async void Parse()
        {
            var config = Configuration.Default.WithDefaultLoader();
            document = await BrowsingContext.New(config).OpenAsync(address);
            var cards = document.QuerySelectorAll(".product-card");

            Console.WriteLine(cards.Length);
        }

        public async Task<ProductInfo> ParseSite()
        {
            var config = Configuration.Default.WithDefaultLoader();
            document = await BrowsingContext.New(config).OpenAsync(address);
            var record = new ProductInfo();

            var cellSelector = ".select-city-link";
            var cell = document.QuerySelector(cellSelector).QuerySelector("a");
            record.Region = cell.TextContent.Trim();

            cellSelector = ".breadcrumb";
            var breadcrumbs = document.QuerySelector(cellSelector).QuerySelectorAll("a.breadcrumb-item");
            var buffer = new List<string>();

            foreach (var breadcrumb in breadcrumbs)
            {
                buffer.Add(breadcrumb.TextContent.Trim());
            }

            record.ProductName = GetArgument(".detail-name");
            buffer.Add(record.ProductName);
            record.BreadCrumbs = String.Join(",", buffer.ToArray());
            record.PriceCurrent = GetArgument(".price");
            record.PriceOld = GetArgument(".old-price");
            record.Availability = GetArgument(".ok", "Нет в наличии");
            var images = new List<string>();

            foreach (var image in document.Images)
            {
                if (image.GetAttribute("class") == "img-fluid")
                    images.Add(image.Source.Trim());
            }

            record.Images = String.Join(",", images.ToArray());
            record.Url = document.Url;

            return record;
        }

        public string GetArgument(string cellselector, string text = "")
        {
            var cell = document.QuerySelector(cellselector);
            var result = (cell != null) ? cell.TextContent.Trim() : text;

            return result;
        }
    }
}
