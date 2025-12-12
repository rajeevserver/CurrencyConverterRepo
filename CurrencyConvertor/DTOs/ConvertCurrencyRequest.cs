using System.ComponentModel.DataAnnotations;

namespace CurrencyConvertor.DTOs
{
    public class ConvertCurrencyRequest
    {
        [Required(ErrorMessage = "sourceCurrency is required.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "sourceCurrency must be exactly 3 characters.")]
        public string SourceCurrency { get; set; } = string.Empty;

        [Required(ErrorMessage = "targetCurrency is required.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "targetCurrency must be exactly 3 characters.")]
        public string TargetCurrency { get; set; } = string.Empty;

        [Range(0.0001, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }
}
