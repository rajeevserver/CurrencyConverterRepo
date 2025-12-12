# Currency Converter API
## Overview

Currency Converter API is an ASP.NET Core Web API that allows you to convert amounts between different currencies using predefined exchange rates. The API is designed with strong typing, unit testing, logging, and robust error handling.

## Key features include:

Convert between supported currencies.

Automatic validation for currency codes (ISO 4217, 3-letter codes) and amount.

Returns descriptive errors for invalid input or unsupported currencies.

Logging of successful conversions and errors via Serilog.

Configurable exchange rates via JSON file with automatic reload support.

Swagger/OpenAPI documentation for all endpoints.

## Features

Validate sourceCurrency and targetCurrency are exactly 3 letters.

Validate amount is greater than zero.

Returns 400 Bad Request for:

- Missing parameters

- Invalid currency codes

- Invalid amount

- Uses IOptionsMonitor<T> to read exchange rates from Data/exchange-rates.json.

- Supports automatic reload of exchange rates when the JSON file changes.

