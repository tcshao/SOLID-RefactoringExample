using System;
using System.Net.Mail;
using System.Net.Http;

namespace Initial
{
    class Program
    {
        static void Main(string[] args)
        {
            var beginDate = DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0));
            decimal total = 0;

            // get a total of the prices from the last 7 days
            while (beginDate < DateTime.Now)
            {
                var response = new HttpClient().GetAsync("http://www.null.com/" + beginDate.ToShortDateString()).Result;

                int price = int.Parse(response.Content.ReadAsStringAsync().Result);

                total += price;
                beginDate = beginDate.Add(new TimeSpan(1, 0, 0, 0));
            }

            // calculate the average from the total
            var average = total / 7;


            var emailMessage = new MailMessage("from@null.com", "to@null.com")
            {
                Body = "The forcasted price is " + average.ToString()
            };

            new SmtpClient("smtp.null.com").Send(emailMessage);

        }
    }
}