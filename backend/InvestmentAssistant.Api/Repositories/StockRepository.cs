using Microsoft.EntityFrameworkCore;
using InvestmentAssistant.Api.Models.Entities;
using InvestmentAssistant.Api.Infrastructure.Database;

namespace InvestmentAssistant.Api.Repositories;

public class StockRepository : IStockRepository
{
    private readonly AppDbContext _dbContext;

    public StockRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Stock?> GetBySymbolAsync(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            return null;

        return await _dbContext.Set<Stock>()
            .FirstOrDefaultAsync(s => s.Symbol == symbol.ToUpper());
    }

    public async Task<Stock> CreateOrUpdateAsync(Stock stock)
    {
        var existing = await GetBySymbolAsync(stock.Symbol);

        if (existing is null)
        {
            _dbContext.Set<Stock>().Add(stock);
        }
        else
        {
            existing.CurrentPrice = stock.CurrentPrice;
            existing.OpenPrice = stock.OpenPrice;
            existing.HighPrice = stock.HighPrice;
            existing.LowPrice = stock.LowPrice;
            existing.PreviousClosePrice = stock.PreviousClosePrice;
            existing.LastUpdatedAt = stock.LastUpdatedAt;
            _dbContext.Set<Stock>().Update(existing);
        }

        await _dbContext.SaveChangesAsync();

        return existing ?? stock;
    }
}