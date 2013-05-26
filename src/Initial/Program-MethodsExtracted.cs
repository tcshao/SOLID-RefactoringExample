using System;
using System.Net.Mail;
using System.Net.Http;
namespace Initial
{
    class Program_MethodsExtracted
    {
        static int GetPriceFromWebService(string url)
        {
            var response = new HttpClient().GetAsync(url).Result;
            return int.Parse(response.Content.ReadAsStringAsync().Result);
        }

        static decimal GetForecastedPrice(decimal total)
        {
            return total / 7;
        }

        static void SendEMail(string toAddress, string fromAddress, string mailServer, string body)
        {
            var emailMessage = new MailMessage(fromAddress, toAddress)
            {
                Body = body
            };

            new SmtpClient(mailServer).Send(emailMessage);
        }

        static void Main_MethodsExtracted()
        {
            var beginDate = DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0));
            decimal total = 0;

            // get a total of the prices from the last 7 days
            while (beginDate < DateTime.Now)
            {
                var price = GetPriceFromWebService("http://www.null.com/" + beginDate.ToShortDateString());

                total += price;
                beginDate = beginDate.Add(new TimeSpan(1, 0, 0, 0));
            }

            // calculate the average from the total
            var forecastedPrice = GetForecastedPrice(total);

            SendEMail("to@null.com", "from@null.com", "smtp.null.com", "The forcasted price is " + forecastedPrice.ToString());
        }
    }
}
