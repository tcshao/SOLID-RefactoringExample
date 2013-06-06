using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ISP;
using Moq;

namespace Tests
{
    public class MyTests
    {
        [Fact]
        public void forecasted_price_should_be_the_average()
        {
            var expected = 25;

            var actual = Program.GetForecastedPrice(125, 5);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void price_service_should_return_a_price_for_a_date()
        {
            var mockPriceService = new Mock<IPriceService>();
            mockPriceService.Setup(service => service.GetPriceForDate(It.IsAny<DateTime>())).Returns(5);

            var expected = 5;
            var actual = mockPriceService.Object.GetPriceForDate(DateTime.Now);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void the_gettotalprices_function_should_return_the_total_prices_for_a_number_of_days()
        {
            var mockPriceService = new Mock<IPriceService>();
            mockPriceService.Setup(service => service.GetPriceForDate(It.IsAny<DateTime>())).Returns(25);

            var expected = 125;
            var actual = Program.GetTotalPrices(mockPriceService.Object, 5);

            Assert.Equal(expected, actual);
        }
    }
}
