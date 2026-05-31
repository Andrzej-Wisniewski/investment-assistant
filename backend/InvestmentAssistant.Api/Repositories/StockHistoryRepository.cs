using InvestmentAssistant.Api.Infrastructure.Database;
using InvestmentAssistant.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvestmentAssistant.Api.Repositories;

public class StockHistoryRepository : IStockHistoryRepository
{
    private readonly AppDbContext _context;

    public StockHistoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StockHistory>> GetLastNDaysAsync(string symbol, int days)
    {
        return await _context.Set<StockHistory>()
            .Where(h => h.Stock.Symbol == symbol.ToUpper())
            .OrderByDescending(h => h.Date)
            .Take(days)
            .OrderBy(h => h.Date)
            .ToListAsync();
    }

    public async Task AddAsync(StockHistory history)
    {
        await _context.Set<StockHistory>().AddAsync(history);
        await _context.SaveChangesAsync();
    }

    public async Task<StockHistory?> GetLatestAsync(string symbol)
    {
        return await _context.Set<StockHistory>()
            .Where(h => h.Stock.Symbol == symbol.ToUpper())
            .OrderByDescending(h => h.Date)
            .FirstOrDefaultAsync();
    }
}