using System;

namespace ISP
{
    public class Program
    {
        static void Main(string[] args)
        {
            var priceService = PriceService.GetPriceService(args[0]);
            var emailService = new EmailService(args[1]);

            var total = GetTotalPrices(priceService, days: 7);
            var forecastedPrice = GetForecastedPrice(total, days: 7);

            emailService.Send(
                "from@null.com", 
                "to@null.com",
                "The forecasted price is " + forecastedPrice.ToString());
        }

        public static decimal GetTotalPrices(IPriceService service, int days)
        {
            var beginDate = DateTime.Now.Subtract(new TimeSpan(days, 0, 0, 0));
            decimal total = 0;

            // get a total of the prices from the last 7 days
            while (beginDate.Date < DateTime.Now.Date)
            {
                int price = service.GetPriceForDate(beginDate);

                total += price;
                beginDate = beginDate.Add(new TimeSpan(1, 0, 0, 0));
            }

            return total;
        }

        public static decimal GetForecastedPrice(decimal totalPrices, int days)
        {
            // calculate the average from the total
            return totalPrices / days;
        }
    }
}