using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AngleSharp_parser
{
    public static class Document
    {
        public static void Create(string path = "test.csv")
        {
            using (var stream = new StreamWriter(path, false, Encoding.UTF8))
            { 
                stream.WriteLine("Region;Product_name;Bread_crumbs;Price_current;Price_old;" +
                    "Availability;Images_links;Url");
            }
        }

        public static void MakeRecord(TextWriter stream, ProductInfo record)
        {
            stream.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7}",
            record.Region, record.ProductName, record.BreadCrumbs, record.PriceCurrent,
            record.PriceOld, record.Availability, record.Images, record.Url);
        }
    }
}
