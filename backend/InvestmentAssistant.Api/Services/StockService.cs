using InvestmentAssistant.Api.Infrastructure.Cache;
using InvestmentAssistant.Api.Models.Responses;
using InvestmentAssistant.Api.Repositories;

namespace InvestmentAssistant.Api.Services;

public class StockService : IStockService
{
    private readonly IAlphaVantageService _alphaVantageService;
    private readonly IStockRepository _stockRepository;

    private readonly ICacheService _cacheService;

    private readonly ILogger<StockService> _logger;

    public StockService(IAlphaVantageService alphaVantageService, IStockRepository stockRepository, ICacheService cacheService, ILogger<StockService> logger)
    {
        _alphaVantageService = alphaVantageService;
        _stockRepository = stockRepository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<StockResponse?> GetStockDataAsync(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            return null;

        symbol = symbol.ToUpper();
        var cacheKey = $"stock:{symbol}";

        var cachedData = await _cacheService.GetAsync<StockResponse>(cacheKey);
        if (cachedData is not null)
        {
            _logger.LogInformation("Znaleziono dane w  pamięci podręcznej dla symbolu {Symbol}", symbol);
            return cachedData;
        }

        _logger.LogInformation($"Nie znaleziono danych w pamięci podręcznej dla symbolu {symbol}. Pobieranie z Alpha Vantage...", symbol);
        var stockData = await _alphaVantageService.GetLatestStockDataAsync(symbol);

        if (stockData is null)
        {
            _logger.LogWarning("Nie można pobrać {symbol} z Alpha Vantage", symbol);
            return null;
        }

        var savedStock = await _stockRepository.CreateOrUpdateAsync(stockData);

        var response = new StockResponse(
            Symbol: savedStock.Symbol,
            CurrentPrice: savedStock.CurrentPrice,
            OpenPrice: savedStock.OpenPrice,
            HighPrice: savedStock.HighPrice,
            LowPrice: savedStock.LowPrice,
            PreviousClosePrice: savedStock.PreviousClosePrice,
            LastUpdatedAt: savedStock.LastUpdatedAt
     );

        await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(5));

        return response;
    }
}