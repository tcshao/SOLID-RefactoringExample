using System;
using System.Net.Mail;
using System.Net.Http;
using System.IO;

namespace LSP
{
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
    class Program
    {
        static void Main(string[] args)
        {
            var priceService = PriceServiceFactory.GetPriceService("34.124.42.2");
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