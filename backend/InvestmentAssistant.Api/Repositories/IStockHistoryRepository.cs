using InvestmentAssistant.Api.Models.Entities;

namespace InvestmentAssistant.Api.Repositories;

public interface IStockHistoryRepository
{
    Task<IEnumerable<StockHistory>> GetLastNDaysAsync(string symbol, int days);
    Task AddAsync(StockHistory history);
    Task<StockHistory?> GetLatestAsync(string symbol);
}