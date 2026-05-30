using System.Text.Json;
using InvestmentAssistant.Api.Models.Entities;
using InvestmentAssistant.Api.Models.Responses;

namespace InvestmentAssistant.Api.Services;

/// <summary>
/// Implementacja komunikacji z Alpaca Markets API
/// </summary>
public class AlpacaService : IAlpacaService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AlpacaService> _logger;
    private readonly string _apiKey;
    private readonly string _baseUrl;

    public AlpacaService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<AlpacaService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("Alpaca");
        _logger = logger;
        _apiKey = configuration["Alpaca:ApiKey"] ?? string.Empty;
        _baseUrl = configuration["Alpaca:BaseUrl"] ?? "https://paper-api.alpaca.markets";
    }

    /// <summary>
    /// Pobiera najnowsze dane cenowe dla symbolu.
    /// </summary>
    public async Task<Stock?> GetLatestStockDataAsync(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            _logger.LogWarning("Symbol jest pusty");
            return null;
        }

        try
        {
            var url = $"{_baseUrl}/v1/stocks/{symbol}/quotes/latest";

            // Przygotuj żądanie
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("APCA-API-KEY-ID", _apiKey);

            // Wyślij żądanie
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Alpaca API zwrócił błąd: {response.StatusCode} dla {symbol}");
                return null;
            }

            // Deserializuj odpowiedź
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var jsonData = JsonSerializer.Deserialize<JsonElement>(content, options);

            // Sprawdź, czy jest w odpowiedzi pole "quote"
            if (!jsonData.TryGetProperty("quote", out var quoteElement))
            {
                _logger.LogWarning($"Brak pola 'quote' w odpowiedzi dla {symbol}");
                return null;
            }

            // Deserializuj quote
            var quoteJson = quoteElement.GetRawText();
            var quote = JsonSerializer.Deserialize<AlpacaQuoteResponse>(quoteJson, options);

            if (quote is null)
            {
                _logger.LogWarning($"Nie mogę deserializować quote dla {symbol}");
                return null;
            }

            // Stwórz encję Stock
            var stock = new Stock
            {
                Symbol = symbol.ToUpper(),
                CurrentPrice = quote.Ap, 
                OpenPrice = quote.Op ?? quote.Ap, 
                HighPrice = quote.Ah ?? quote.Ap, 
                LowPrice = quote.Al ?? quote.Ap, 
                PreviousClosePrice = quote.Pc, 
                LastUpdatedAt = DateTime.UtcNow
            };

            _logger.LogInformation($"Pomyślnie pobrano dane dla {symbol}: {stock.CurrentPrice}");
            return stock;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Błąd przy pobieraniu danych z Alpaca API dla {symbol}: {ex.Message}");
            return null;
        }
    }
}

/// <summary>
/// Mapowanie na Latest Quote response z Alpaca API.
/// </summary>
public class AlpacaQuoteResponse
{
    [System.Text.Json.Serialization.JsonPropertyName("ap")]
    public decimal Ap { get; set; } 

    [System.Text.Json.Serialization.JsonPropertyName("as")]
    public long As { get; set; } 

    [System.Text.Json.Serialization.JsonPropertyName("bp")]
    public decimal Bp { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("bs")]
    public long Bs { get; set; } 

    [System.Text.Json.Serialization.JsonPropertyName("pc")]
    public decimal Pc { get; set; } 

    [System.Text.Json.Serialization.JsonPropertyName("op")]
    public decimal? Op { get; set; } 

    [System.Text.Json.Serialization.JsonPropertyName("ah")]
    public decimal? Ah { get; set; } 

    [System.Text.Json.Serialization.JsonPropertyName("al")]
    public decimal? Al { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("t")]
    public long T { get; set; } 
}