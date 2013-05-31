using System;
using System.Net.Mail;
using System.Net.Http;
using System.IO;

namespace OCP
{
    class Program
    {
        // This interface defines the behavior of the classes, which ensures they will still behave the same
        // even though they are modified.
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


        public class EmailService
        {
            public string SmtpServer { get; set; }

            public EmailService(string server)
            {
                SmtpServer = server;
            }

            public void Send(string toAddress, string fromAddress, string body)
            {
                var emailMessage = new MailMessage(fromAddress, toAddress)
                {
                    Body = body
                };

                new SmtpClient(SmtpServer).Send(emailMessage);
            }
        }

        static void Main(string[] args)
        {
            IPriceService priceService = new PriceServiceWeb("http://www.null.com");
            var emailService = new EmailService("smtp.null.com");

            var beginDate = DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0));
            decimal total = 0;

            // get a total of the prices from the last 7 days
            while (beginDate < DateTime.Now)
            {
                int price = priceService.GetPriceForDate(beginDate);

                total += price;
                beginDate = beginDate.Add(new TimeSpan(1, 0, 0, 0));
            }

            // calculate the average from the total
            var average = total / 7;

            emailService.Send("from@null.com", "to@null.com", "The forecasted price is " + average.ToString());

        }
    }
}