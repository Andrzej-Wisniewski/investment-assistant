using InvestmentAssistant.Api.Models.Responses;

namespace InvestmentAssistant.Api.Services;

public interface IStockService
{
    Task<StockResponse?> GetStockDataAsync(string symbol);
}