using InvestmentAssistant.Api.Models.Entities;

namespace InvestmentAssistant.Api.Repositories;

public interface IStockRepository
{
    Task<Stock?> GetBySymbolAsync(string symbol);
    Task<Stock> CreateOrUpdateAsync(Stock stock);
}