namespace InvestmentAssistant.Api.Models.Entities;

public class TradingDecision
{
    public MarketSignal Signal { get; set; }
    public double Confidence { get; set; }
    public double FinalScore { get; set; }
}