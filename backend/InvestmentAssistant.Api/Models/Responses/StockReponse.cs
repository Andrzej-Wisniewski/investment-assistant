namespace InvestmentAssistant.Api.Models.Responses;

/// <summary>
/// Reprezentacja akcji w API
/// </summary>
public record StockResponse(
    string Symbol,
    decimal CurrentPrice,
    decimal OpenPrice,
    decimal HighPrice,
    decimal LowPrice,
    decimal PreviousClosePrice,
    DateTime LastUpdatedAt
);