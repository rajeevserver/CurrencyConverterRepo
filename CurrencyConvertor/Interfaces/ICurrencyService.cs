using CurrencyConvertor.DTOs;

namespace CurrencyConvertor.Interfaces
{
    public interface ICurrencyService
    {
        CurrencyConversionResponse? Convert(string sourceCurrency, string targetCurrency, decimal amount);
    }
}
