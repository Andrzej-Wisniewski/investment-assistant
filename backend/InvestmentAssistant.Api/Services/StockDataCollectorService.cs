using InvestmentAssistant.Api.Repositories;

namespace InvestmentAssistant.Api.Services;

/// <summary>
/// Implementacja serwisu zbierającego dane akcji w tle
/// </summary>
public class StockDataCollectorService : IStockDataCollectorService
{
    private readonly IStockService _stockService;
    private readonly ILogger<StockDataCollectorService> _logger;

    public DateTime? LastSuccessfulUpdate { get; private set; }

    // Lista symboli do monitorowania
    private readonly string[] _defaultSymbols = 
    {
        "AAPL", "GOOGL", "MSFT", "AMZN", "TSLA",
        "META", "NVDA", "INTL", "CSCO", "ADBE"
    };

    public StockDataCollectorService(
        IStockService stockService,
        ILogger<StockDataCollectorService> logger)
    {
        _stockService = stockService;
        _logger = logger;
    }

    public async Task CollectStockDataAsync(IEnumerable<string> symbols)
    {
        var symbolList = symbols?.ToList() ?? _defaultSymbols.ToList();

        _logger.LogInformation($"Początek zbierania danych dla {symbolList.Count} symboli");

        var successCount = 0;
        var failureCount = 0;

        foreach (var symbol in symbolList)
        {
            try
            {
                var stock = await _stockService.GetStockDataAsync(symbol);
                if (stock is not null)
                {
                    successCount++;
                    _logger.LogInformation($"Pomyślnie pobrano {symbol}");
                }
                else
                {
                    failureCount++;
                    _logger.LogWarning($"Nie mogę pobrać {symbol}");
                }

                await Task.Delay(100);
            }
            catch (Exception ex)
            {
                failureCount++;
                _logger.LogError($"Błąd przy pobieraniu {symbol}: {ex.Message}");
            }
        }

        LastSuccessfulUpdate = DateTime.UtcNow;
        _logger.LogInformation($"Zakończono zbieranie: {successCount} sukces, {failureCount} błędów");
    }
}