using InvestmentAssistant.Api.Models.Entities;
using InvestmentAssistant.Api.Repositories;

namespace InvestmentAssistant.Api.Services;

/// <summary>
/// Oblicza wskaźniki techniczne (RSI, SMA) z historii cen
/// </summary>
public class TechnicalIndicatorService
{
    private readonly IStockRepository _stockRepository;
    private readonly IStockHistoryRepository _historyRepository;
    private readonly ILogger<TechnicalIndicatorService> _logger;

    public TechnicalIndicatorService(
        IStockRepository stockRepository,
        IStockHistoryRepository historyRepository,
        ILogger<TechnicalIndicatorService> logger)
    {
        _stockRepository = stockRepository;
        _historyRepository = historyRepository;
        _logger = logger;
    }

    /// <summary>
    /// Oblicza RSI (Relative Strength Index) na podstawie ostatnich 14 dni
    /// </summary>
    public async Task<double> CalculateRsiAsync(string symbol)
    {
        const int rsiPeriod = 14;
        
        var history = (await _historyRepository.GetLastNDaysAsync(symbol, rsiPeriod + 1))
            .ToList();

        if (history.Count < rsiPeriod)
        {
            _logger.LogWarning($"Zbyt mało danych do obliczenia RSI dla {symbol} ({history.Count}/{rsiPeriod})");
            return 50.0; 
        }

        var prices = history.Select(h => (double)h.ClosePrice).ToList();

        // Obliczamy zmiany ceny
        var gains = new List<double>();
        var losses = new List<double>();

        for (int i = 1; i < prices.Count; i++)
        {
            double change = prices[i] - prices[i - 1];
            if (change > 0)
            {
                gains.Add(change);
                losses.Add(0);
            }
            else
            {
                gains.Add(0);
                losses.Add(Math.Abs(change));
            }
        }

        double avgGain = gains.Take(rsiPeriod).Average();
        double avgLoss = losses.Take(rsiPeriod).Average();

        if (avgLoss == 0)
            return avgGain > 0 ? 100.0 : 50.0;

        double rs = avgGain / avgLoss;
        double rsi = 100.0 - (100.0 / (1.0 + rs));

        _logger.LogInformation($"RSI dla {symbol}: {rsi:F2}");
        return rsi;
    }

    /// <summary>
    /// Oblicza SMA20 (Simple Moving Average 20-okresową)
    /// </summary>
    public async Task<double> CalculateSma20Async(string symbol)
    {
        const int smaPeriod = 20;

        var history = (await _historyRepository.GetLastNDaysAsync(symbol, smaPeriod))
            .ToList();

        if (history.Count < smaPeriod)
        {
            _logger.LogWarning($"Zbyt mało danych do obliczenia SMA20 dla {symbol} ({history.Count}/{smaPeriod})");
            
            var stock = await _stockRepository.GetBySymbolAsync(symbol);
            return stock != null ? (double)stock.CurrentPrice : 0.0;
        }

        double sma20 = history
            .Select(h => (double)h.ClosePrice)
            .Average();

        _logger.LogInformation($"SMA20 dla {symbol}: {sma20:F2}");
        return sma20;
    }
}