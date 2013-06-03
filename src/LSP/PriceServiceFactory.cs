using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.IO;

namespace LSP
{
    public static class PriceServiceFactory
    {
        public static IPriceService GetPriceService(string targetInfo)
        {
            var textFilePattern = @"\.txt\z";
            var ipAddressPattern = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";

            if(Regex.IsMatch(targetInfo, textFilePattern))
            {
                return new PriceServiceFile(targetInfo);
            }
            
            if(Regex.IsMatch(targetInfo, ipAddressPattern))
            {
                return new PriceServiceWeb(targetInfo);
            }

            return null;
        }
    }

    public interface IPriceService
    {
        int GetPriceForDate(DateTime date);
    }

    public class PriceServiceWeb : IPriceService
    {
        public string Uri { get; set; }

        public PriceServiceWeb(string uri)
        {
            Uri = uri;
        }

        public int GetPriceForDate(DateTime date)
        {
            var response = new HttpClient().GetAsync(Uri + date.ToShortDateString()).Result;

            return int.Parse(response.Content.ReadAsStringAsync().Result);
        }
    }

    public class PriceServiceFile : IPriceService
    {
        public string FileName { get; set; }

        public PriceServiceFile(string fileName)
        {
            FileName = fileName;
        }

        public int GetPriceForDate(DateTime date)
        {
            // reads from a CSV and gets data
            int price = 0;

            using (var sr = new StreamReader(FileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var lineDate = line.Split(',')[0];
                    var linePrice = line.Split(',')[1];

                    if (lineDate == date.ToShortDateString())
                    {
                        price = int.Parse(linePrice);
                        break;
                    }
                }
            }

            return price;
        }
    }
}
