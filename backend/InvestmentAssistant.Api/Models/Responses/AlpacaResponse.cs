using System.Text.Json.Serialization;

namespace InvestmentAssistant.Api.Models.Responses;

/// <summary>
/// Reprezentacja odpowiedzi z Alpaca API dla danych cenowych
/// </summary>
public class AlpacaResponse
{
    [JsonPropertyName("t")]
    public long Timestamp { get; set; }

    [JsonPropertyName("o")]
    public decimal Open { get; set; }

    [JsonPropertyName("h")]
    public decimal High { get; set; }

    [JsonPropertyName("l")]
    public decimal Low { get; set; }

    [JsonPropertyName("c")]
    public decimal Close { get; set; }

    [JsonPropertyName("v")]
    public long Volume { get; set; }

    [JsonPropertyName("vw")]
    public decimal VolumeWeighted { get; set; }

    [JsonPropertyName("n")]
    public long NumberOfTransactions { get; set; }
}

/// <summary>
/// Cała odpowiedź z Alpaca API.
/// </summary>
public class AlpacaApiResponse
{
    [JsonPropertyName("bars")]
    public Dictionary<string, List<AlpacaResponse>> Bars { get; set; } = new();

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("next_page_token")]
    public string? NextPageToken { get; set; }
}