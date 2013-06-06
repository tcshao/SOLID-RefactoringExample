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
    public class PriceServiceFixture : IDisposable
    {
        Mock<IPriceService> _priceService;

        public PriceServiceFixture()
        {
            var mockPriceService = new Mock<IPriceService>();
            mockPriceService.Setup(service => service.GetPriceForDate(It.IsAny<DateTime>())).Returns(5);

            _priceService = mockPriceService;
        }

        public IPriceService GetService()
        {
            return _priceService.Object;
        }

        public void Dispose()
        {
            _priceService = null;
        }
    }
    public class RefactoredTests : IUseFixture<PriceServiceFixture>
    {
        IPriceService _priceService;

        public void SetFixture(PriceServiceFixture priceService)
        {
            _priceService = priceService.GetService();
        }

        [Fact]
        [Trait("Category","Refactored")]
        public void forecasted_price_should_be_the_average()
        {
            var expected = 25;
            var actual = Program.GetForecastedPrice(125, 5);

            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("Category", "Refactored")]
        public void price_service_should_return_a_price_for_a_date()
        {
            var expected = 5;
            var actual = _priceService.GetPriceForDate(DateTime.Now);

            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("Category", "Refactored")]
        public void the_gettotalprices_function_should_return_the_total_prices_for_a_number_of_days()
        {
            var expected = 25;
            var actual = Program.GetTotalPrices(_priceService, 5);

            Assert.Equal(expected, actual);
        }
    }
}
