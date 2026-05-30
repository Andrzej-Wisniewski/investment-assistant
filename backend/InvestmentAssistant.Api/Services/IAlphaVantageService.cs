using InvestmentAssistant.Api.Models.Entities;

namespace InvestmentAssistant.Api.Services;

/// <summary>
/// Abstrakcja komunikacji z Alpha Vantage API
/// </summary>
public interface IAlphaVantageService
{
    /// <summary>
    /// Pobiera najnowsze dane cenowe dla akcji
    /// </summary>
    Task<Stock?> GetLatestStockDataAsync(string symbol);
}