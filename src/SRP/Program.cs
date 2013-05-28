using System;
using System.Net.Mail;
using System.Net.Http;

namespace Initial
{
    class Program
    {
        public class PriceService
        {
            public string Uri { get; set; }

            public PriceService(string uri)
            {
                Uri = uri;
            }

            public int GetPriceForDate(DateTime date)
            {
                var response = new HttpClient().GetAsync(Uri + date.ToShortDateString()).Result;

                return int.Parse(response.Content.ReadAsStringAsync().Result);
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
            var priceService = new PriceService("http://www.null.com");
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