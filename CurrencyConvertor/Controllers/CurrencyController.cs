using CurrencyConvertor.DTOs;
using CurrencyConvertor.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConvertor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ILogger<CurrencyController> _logger;
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ILogger<CurrencyController> logger, ICurrencyService currencyService)
        {
            _logger = logger;
            _currencyService = currencyService;
        }


        [HttpGet("convert")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]

        public IActionResult ConvertCurrency(
        [FromQuery] ConvertCurrencyRequest convertCurrencyRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _currencyService.Convert(convertCurrencyRequest.SourceCurrency, convertCurrencyRequest.TargetCurrency, convertCurrencyRequest.Amount);

                _logger.LogInformation("Converted {Amount} {Source} -> {Target} using rate {Rate}. Output={Converted}",
                    convertCurrencyRequest.Amount, convertCurrencyRequest.SourceCurrency, convertCurrencyRequest.TargetCurrency, result?.ExchangeRate, result?.ConvertedAmount);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                // Handles unsupported/invalid currency codes
                _logger.LogWarning(ex, "Conversion failed due to unsupported currency.");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Handles any unexpected errors
                _logger.LogError(ex, "Unexpected error during conversion.");
                return BadRequest(new { error = "Invalid request." });
            }
        }
    }
}
