using InvestmentAssistant.Api.Models.Entities;

namespace InvestmentAssistant.Api.Services;

/// <summary>
/// Algorytm analizy technicznej z systemem punktacji ważonej
/// </summary>
public class MarketAnalyzer
{
    private const double RsiWeight = 0.6;   
    private const double SmaWeight = 0.4;   

    private const double BuyThreshold = 0.2;    
    private const double SellThreshold = -0.2;  

    /// <summary>
    /// Analizuje wskaźniki techniczne i zwraca sygnał handlowy
    /// </summary>
    public TradingDecision Analyze(double currentPrice, double rsi, double sma20)
    {
        if (currentPrice <= 0 || sma20 <= 0)
            throw new ArgumentException("Cena i SMA muszą być większe od zera");

        double rsiScore = CalculateRsiScore(rsi);
        double smaScore = CalculateSmaScore(currentPrice, sma20);

        // Średnia ważona wskaźników
        double finalScore = (rsiScore * RsiWeight) + (smaScore * SmaWeight);

        var decision = new TradingDecision
        {
            FinalScore = finalScore,
            Confidence = DetermineConfidence(finalScore)
        };

        // Wyznaczenie sygnału na podstawie progów
        if (finalScore >= BuyThreshold)
        {
            decision.Signal = MarketSignal.Buy;
        }
        else if (finalScore <= SellThreshold)
        {
            decision.Signal = MarketSignal.Sell;
        }
        else
        {
            decision.Signal = MarketSignal.Hold;
        }

        return decision;
    }

    /// <summary>
    /// Oblicza score RSI (Relative Strength Index)
    /// </summary>
    private double CalculateRsiScore(double rsi)
    {
        if (rsi < 30)
        {
            // Im niższe RSI, tym silniejszy sygnał kupna [0, 1]
            return (30 - rsi) / 30.0;
        }
        else if (rsi > 70)
        {
            // Im wyższe RSI, tym silniejszy sygnał sprzedaży [-1, 0]
            return -(rsi - 70) / 30.0;
        }

        return 0.0;
    }

    /// <summary>
    /// Oblicza score SMA (Simple Moving Average)
    /// </summary>
    private double CalculateSmaScore(double currentPrice, double sma20)
    {
        double priceDeviation = (currentPrice - sma20) / sma20;

        return Math.Tanh(priceDeviation * 50);
    }

    /// <summary>
    /// Oblicza pewność sygnału
    /// </summary>
    private double DetermineConfidence(double finalScore)
    {
        return Math.Abs(finalScore);
    }
}