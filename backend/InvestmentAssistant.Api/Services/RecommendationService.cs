using InvestmentAssistant.Api.Models.Responses;

namespace InvestmentAssistant.Api.Services;

public class RecommendationService : IRecommendationService
{
    private readonly IStockService _stockService;
    private readonly TechnicalIndicatorService _indicatorService;
    private readonly MarketAnalyzer _analyzer;
    private readonly ILogger<RecommendationService> _logger;

    public RecommendationService(
        IStockService stockService,
        TechnicalIndicatorService indicatorService,
        ILogger<RecommendationService> logger)
    {
        _stockService = stockService;
        _indicatorService = indicatorService;
        _analyzer = new MarketAnalyzer();
        _logger = logger;
    }

    public async Task<RecommendationResponse?> GetRecommendationAsync(string symbol)
    {
        var stock = await _stockService.GetStockDataAsync(symbol);

        if (stock is null)
        {
            _logger.LogWarning($"Nie mogę pobrać danych dla {symbol}");
            return null;
        }

        // Obliczamy rzeczywiste wskaźniki
        double rsi = await _indicatorService.CalculateRsiAsync(symbol);
        double sma20 = await _indicatorService.CalculateSma20Async(symbol);

        var decision = _analyzer.Analyze((double)stock.CurrentPrice, rsi, sma20);

        return new RecommendationResponse
        {
            Symbol = symbol.ToUpper(),
            Signal = decision.Signal.ToString(),
            Confidence = Math.Round(decision.Confidence, 2),
            FinalScore = Math.Round(decision.FinalScore, 2),
            TechnicalIndicators = new TechnicalIndicatorsDto
            {
                CurrentPrice = (double)stock.CurrentPrice,
                Rsi = Math.Round(rsi, 2),
                Sma20 = Math.Round(sma20, 2)
            },
            Reasoning = GenerateReasoning(decision, rsi, (double)stock.CurrentPrice, sma20),
            AnalyzedAt = DateTime.UtcNow
        };
    }

    private string GenerateReasoning(
        Models.Entities.TradingDecision decision,
        double rsi,
        double currentPrice,
        double sma20)
    {
        var reasons = new List<string>();

        if (rsi < 30)
            reasons.Add($"RSI {rsi:F1} wskazuje wyprzedanie (< 30)");
        else if (rsi > 70)
            reasons.Add($"RSI {rsi:F1} wskazuje wykupienie (> 70)");
        else
            reasons.Add($"RSI {rsi:F1} w strefie neutralnej");

        double deviation = ((currentPrice - sma20) / sma20) * 100;
        if (deviation > 0)
            reasons.Add($"Cena powyżej SMA20 o {deviation:F1}%");
        else
            reasons.Add($"Cena poniżej SMA20 o {Math.Abs(deviation):F1}%");

        reasons.Add($"Werdykt: {decision.Signal.ToString().ToUpper()} (pewność: {decision.Confidence:P0})");

        return string.Join(" | ", reasons);
    }
}