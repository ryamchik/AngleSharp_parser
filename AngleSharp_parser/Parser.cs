using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Html.Dom;

namespace AngleSharp_parser
{
    public class Parser
    {
        string address = string.Empty;
        AngleSharp.Dom.IDocument document;
        AngleSharp.IConfiguration config;
        const string HOST = "https://";
        const string URL = "www.toy.ru";

        public Parser(string address)
        {
            this.address = address;
            this.config = Configuration.Default.WithDefaultLoader();
        }

        public async Task<List<ProductInfo>> Parse()
        {
            document = await BrowsingContext.New(config).OpenAsync(address);
            var pages = document.QuerySelectorAll(".page-item");
            var records = new List<ProductInfo>();
            var lastAddress = HOST + URL + pages[pages.Length - 2].QuerySelector("a").GetAttribute("href");
            var nextAddress = HOST + URL + pages[pages.Length - 1].QuerySelector("a").GetAttribute("href");
            var currentAddress = address;
            var counter = 2;
            
            while (counter <= 11)
            {
                var links = new List<string>();
                var cards = document.QuerySelectorAll(".product-card");

                foreach (var card in cards)
                {
                    links.Add(card.QuerySelector("meta").GetAttribute("content"));
                }

                var tasks = new List<Task>();

                foreach (var link in links)
                {
                    tasks.Add(ParseSite(link));
                }

                while (tasks.Count > 0)
                {
                    Task<ProductInfo> finishedTask = (Task<ProductInfo>)await Task.WhenAny(tasks);
                    records.Add(finishedTask.Result);
                    tasks.Remove(finishedTask);
                }

                currentAddress = nextAddress;
                counter++;
                document = await BrowsingContext.New(config).OpenAsync(currentAddress);
                nextAddress = nextAddress[0..^1] + Convert.ToString(counter);
                Console.WriteLine(currentAddress);
                Console.WriteLine(nextAddress);
                Console.WriteLine(records.Count);
            }

            return records;
        }

        public async Task<ProductInfo> ParseSite(string path)
        {
            var document = await BrowsingContext.New(config).OpenAsync(path);
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

            string GetArgument(string cellselector, string text = "")
            {
                var cell = document.QuerySelector(cellselector);
                var result = (cell != null) ? cell.TextContent.Trim() : text;
                return result;
            }
        }

        public async void Change(string region)
        {
            var _context = BrowsingContext.New(config);
            document = await _context.OpenAsync(address);
            (document.QuerySelector(".select-city-link").QuerySelector("a") as IHtmlElement).DoClick();
            document.QuerySelector("#region").SetAttribute("style", "display: inline-block;");
            document.QuerySelector(".select-city-block").SetAttribute("value", region);

            (document.QuerySelector("button[id=\"savecity\"]") as IHtmlElement).DoClick();
            Task.Delay(5000);
            
            Console.WriteLine(document.QuerySelector(".select-city-link").QuerySelector("a").TextContent.Trim());
        }
    }
}
