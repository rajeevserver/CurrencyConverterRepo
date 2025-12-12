using CurrencyConvertor.Configurations;
using CurrencyConvertor.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace CurrencyConvertor.Tests
{
    public class CurrencyServiceTest
    {
        private CurrencyService CreateService(Dictionary<string, decimal> rates)
        {
            var monitorMock = new Mock<IOptionsMonitor<ExchangeRateOptions>>();

            monitorMock
                .Setup(m => m.CurrentValue)
                .Returns(new ExchangeRateOptions
                {
                    ExchangeRates = rates
                });

            return new CurrencyService(monitorMock.Object, NullLogger<CurrencyService>.Instance);
        }

        [Fact]
        public void Convert_ValidRate_ReturnsCorrectAmount()
        {
            var svc = CreateService(new Dictionary<string, decimal>
            {
                ["USD_TO_INR"] = 80
            });

            var result = svc.Convert("USD", "INR", 10);

            Assert.Equal(800, result?.ConvertedAmount);
        }

        [Fact]
        public void Convert_SameCurrency_ReturnsSameAmount()
        {

            var svc = CreateService(new Dictionary<string, decimal>
            {
                ["USD_TO_INR"] = 80
            });

            var result = svc.Convert("INR", "INR", 100);

            Assert.Equal(100, result?.ConvertedAmount);
        }

        [Fact]
        public void Convert_UnsupportedCurrency_Throws()
        {
            var svc = CreateService(new Dictionary<string, decimal>());

            Assert.Throws<KeyNotFoundException>(() =>
                svc.Convert("USD", "XYZ", 100));
        }
    }
}