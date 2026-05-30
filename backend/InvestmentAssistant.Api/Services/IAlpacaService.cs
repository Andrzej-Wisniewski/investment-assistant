using InvestmentAssistant.Api.Models.Entities;

namespace InvestmentAssistant.Api.Services;

/// <summary>
/// Abstrakcja komunikacji z Alpaca Markets API
/// </summary>
public interface IAlpacaService
{
    /// <summary>
    /// Pobiera najnowsze dane cenowe dla akcji
    /// </summary>
    Task<Stock?> GetLatestStockDataAsync(string symbol);
}