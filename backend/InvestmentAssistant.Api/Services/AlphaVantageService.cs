using System.Text.Json;
using InvestmentAssistant.Api.Models.Entities;

namespace InvestmentAssistant.Api.Services;

/// <summary>
/// Implementacja komunikacji z Alpha Vantage API.
/// </summary>
public class AlphaVantageService : IAlphaVantageService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AlphaVantageService> _logger;
    private readonly string _apiKey;

    public AlphaVantageService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<AlphaVantageService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("AlphaVantage");
        _logger = logger;
        _apiKey = configuration["AlphaVantage:ApiKey"] ?? string.Empty;
    }

    public async Task<Stock?> GetLatestStockDataAsync(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            _logger.LogWarning("Symbol jest pusty");
            return null;
        }

        try
        {
            var url = $"https://www.alphavantage.co/query?" +
                      $"function=GLOBAL_QUOTE&symbol={symbol}&apikey={_apiKey}";

            _logger.LogInformation($"Wysyłam żądanie do Alpha Vantage dla {symbol}");

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Alpha Vantage zwrócił błąd: {response.StatusCode} dla {symbol}");
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var jsonData = JsonSerializer.Deserialize<JsonElement>(content, options);

            if (jsonData.TryGetProperty("Error Message", out var errorMsg))
            {
                _logger.LogError($"Alpha Vantage błąd: {errorMsg.GetString()}");
                return null;
            }

            if (!jsonData.TryGetProperty("Global Quote", out var quoteElement))
            {
                _logger.LogWarning($"Brak pola 'Global Quote' w odpowiedzi dla {symbol}");
                return null;
            }

            var quote = JsonSerializer.Deserialize<AlphaVantageQuote>(quoteElement.GetRawText(), options);

            if (quote is null || string.IsNullOrWhiteSpace(quote.Symbol))
            {
                _logger.LogWarning($"Pusta odpowiedź dla {symbol}");
                return null;
            }

            decimal.TryParse(quote.Price, out var price);
            decimal.TryParse(quote.Open, out var open);
            decimal.TryParse(quote.High, out var high);
            decimal.TryParse(quote.Low, out var low);
            decimal.TryParse(quote.PreviousClose, out var prevClose);

            var stock = new Stock
            {
                Symbol = symbol.ToUpper(),
                CurrentPrice = price > 0 ? price : prevClose,
                OpenPrice = open > 0 ? open : price,
                HighPrice = high > 0 ? high : price,
                LowPrice = low > 0 ? low : price,
                PreviousClosePrice = prevClose > 0 ? prevClose : price,
                LastUpdatedAt = DateTime.UtcNow
            };

            _logger.LogInformation($"Pomyślnie pobrano dane dla {symbol}: {stock.CurrentPrice}");
            return stock;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Błąd przy pobieraniu danych z Alpha Vantage dla {symbol}: {ex.Message}");
            return null;
        }
    }

    public async Task<List<Stock>> GetDailyTimeSeriesAsync(string symbol)
{
    try
    {
        var url = $"https://www.alphavantage.co/query?" +
                  $"function=TIME_SERIES_DAILY&symbol={symbol}&outputsize=full&apikey={_apiKey}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return new List<Stock>();

        var content = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var jsonData = JsonSerializer.Deserialize<JsonElement>(content, options);

        var stocks = new List<Stock>();

        if (jsonData.TryGetProperty("Time Series (Daily)", out var timeSeries))
        {
            foreach (var day in timeSeries.EnumerateObject())
            {
                var dateStr = day.Name;
                var prices = day.Value;

                if (DateTime.TryParse(dateStr, out var date) &&
                    prices.TryGetProperty("4. close", out var closePrice))
                {
                    stocks.Add(new Stock
                    {
                        Symbol = symbol.ToUpper(),
                        CurrentPrice = decimal.Parse(closePrice.GetString() ?? "0"),
                        LastUpdatedAt = DateTime.UtcNow
                    });
                }
            }
        }

        return stocks.OrderBy(s => s.LastUpdatedAt).ToList();
    }
    catch (Exception ex)
    {
        _logger.LogError($"Błąd przy pobieraniu historii: {ex.Message}");
        return new List<Stock>();
    }
}
}

public class AlphaVantageQuote
{
    [System.Text.Json.Serialization.JsonPropertyName("01. symbol")]
    public string Symbol { get; set; } = string.Empty;

    [System.Text.Json.Serialization.JsonPropertyName("02. price")]
    public string Price { get; set; } = string.Empty;

    [System.Text.Json.Serialization.JsonPropertyName("03. volume")]
    public string Volume { get; set; } = string.Empty;

    [System.Text.Json.Serialization.JsonPropertyName("04. timestamp")]
    public string Timestamp { get; set; } = string.Empty;

    [System.Text.Json.Serialization.JsonPropertyName("05. price")]
    public string Open { get; set; } = string.Empty;

    [System.Text.Json.Serialization.JsonPropertyName("08. previous close")]
    public string PreviousClose { get; set; } = string.Empty;

    [System.Text.Json.Serialization.JsonPropertyName("09. change")]
    public string Change { get; set; } = string.Empty;

    [System.Text.Json.Serialization.JsonPropertyName("high")]
    public string High { get; set; } = string.Empty;

    [System.Text.Json.Serialization.JsonPropertyName("low")]
    public string Low { get; set; } = string.Empty;
}