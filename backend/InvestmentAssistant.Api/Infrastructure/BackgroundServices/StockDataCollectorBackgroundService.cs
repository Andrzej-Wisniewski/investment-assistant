using InvestmentAssistant.Api.Services;

namespace InvestmentAssistant.Api.Infrastructure.BackgroundServices;

/// <summary>
/// Usługa w tle, która zbiera dane akcji w regularnych odstępach
/// </summary>
public class StockDataCollectorBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<StockDataCollectorBackgroundService> _logger;

    // Co ile minut zbierać dane
    private readonly int _intervalMinutes = 15;

    public StockDataCollectorBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<StockDataCollectorBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"StockDataCollectorBackgroundService uruchomiony. Interval: {_intervalMinutes} minut");

        await CollectDataAsync();

        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(_intervalMinutes));

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await CollectDataAsync();
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("StockDataCollectorBackgroundService zatrzymany");
        }
    }

    private async Task CollectDataAsync()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var collectorService = scope.ServiceProvider.GetRequiredService<IStockDataCollectorService>();

            await collectorService.CollectStockDataAsync(null);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Błąd w CollectDataAsync: {ex.Message}");
        }
    }
}