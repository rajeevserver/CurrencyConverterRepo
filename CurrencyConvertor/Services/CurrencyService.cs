using CurrencyConvertor.Configurations;
using CurrencyConvertor.DTOs;
using CurrencyConvertor.Interfaces;
using Microsoft.Extensions.Options;

namespace CurrencyConvertor.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IOptionsMonitor<ExchangeRateOptions> _options;
        private readonly ILogger<CurrencyService> _logger;

        public CurrencyService(IOptionsMonitor<ExchangeRateOptions> options, ILogger<CurrencyService> logger)
        {
            _options = options;
            _logger = logger;
        }


        /// <summary>
        /// Returns Exchange Rate and amount
        /// </summary>
        /// <param name="sourceCurrency"></param>
        /// <param name="targetCurrency"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public CurrencyConversionResponse? Convert(string sourceCurrency, string targetCurrency, decimal amount)
        {
            string key = $"{sourceCurrency}_TO_{targetCurrency}";
            _logger.Log(LogLevel.Information, key, sourceCurrency, targetCurrency);

            // Find all currency codes in your dictionary keys
            var availableCurrencies = _options.CurrentValue.ExchangeRates
                .Keys
                .Select(k => k.Split('_')[0])
                .Union(_options.CurrentValue.ExchangeRates.Keys.Select(k => k.Split('_')[2]))
                .Distinct()
                .ToHashSet();

            if (!availableCurrencies.Contains(sourceCurrency.ToUpper()))
            {
                _logger.LogError($"Source currency '{sourceCurrency.ToUpper()}' is not supported.");
                throw new KeyNotFoundException($"Source currency '{sourceCurrency.ToUpper()}' is not supported.");
            }


            if (!availableCurrencies.Contains(targetCurrency.ToUpper()))
            {
                _logger.LogError($"Target currency '{targetCurrency.ToUpper()}' is not supported.");
                throw new KeyNotFoundException($"Target currency '{targetCurrency.ToUpper()}' is not supported.");
            }

            // Same currency → rate = 1
            if (sourceCurrency.ToUpper() == targetCurrency.ToUpper())
            {
                _logger.LogError($"Target currency '{targetCurrency.ToUpper()}' & Source currency are identical.");
                return new CurrencyConversionResponse
                {
                    ExchangeRate = 1,
                    ConvertedAmount = amount
                };
            }

            if (!_options.CurrentValue.ExchangeRates.TryGetValue(key.ToUpper(), out var rate))
            {
                _logger.LogError($"Exchange rate '{key}' not found.");
                throw new KeyNotFoundException($"Exchange rate '{key}' not found.");
            }

            return new CurrencyConversionResponse
            {
                ExchangeRate = rate,
                ConvertedAmount = rate * amount
            };
        }
    }
}
