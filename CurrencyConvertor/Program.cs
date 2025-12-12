using CurrencyConvertor.Configurations;
using CurrencyConvertor.Interfaces;
using CurrencyConvertor.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Read Serilog settings from appsettings.json
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Load JSON file manually
var filePath = Path.Combine(builder.Environment.ContentRootPath, "Data/exchange-rates.json");
builder.Configuration.AddJsonFile(filePath, optional: false, reloadOnChange: true);


// Register strongly typed config
// The magic line — supports reload!
builder.Services.Configure<ExchangeRateOptions>(
    builder.Configuration.GetSection("ExchangeRateOptions"));


// Services
builder.Services.AddScoped<ICurrencyService, CurrencyService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
