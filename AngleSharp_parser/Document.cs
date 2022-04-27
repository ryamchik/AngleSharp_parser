using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AngleSharp_parser
{
    public static class Document
    {
        public static void Make(List<ProductInfo> records, string path = "test.csv")
        {
            var addability = (File.Exists(path)) ? true : false;

            using (var stream = new StreamWriter(path, addability, Encoding.UTF8))
            {
                stream.WriteLine("Region;Product_name;Bread_crumbs;Price_current;Price_old;" +
                    "Availability;Images_links;Url");

                records.ForEach(delegate (ProductInfo record)
                {
                    Document.Make(record, stream);
                });
            }
        }

        public static void Make(ProductInfo record, StreamWriter stream)
        {
                stream.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7}",
                    record.Region, record.ProductName, record.BreadCrumbs, record.PriceCurrent,
                    record.PriceOld, record.Availability, record.Images, record.Url);
        }
    }
}
