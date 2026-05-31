namespace InvestmentAssistant.Api.Models.Responses;

public class RecommendationResponse
{
    public string Symbol { get; set; } = string.Empty;
    public string Signal { get; set; } = string.Empty;
    public double Confidence { get; set; }
    public double FinalScore { get; set; }
    public TechnicalIndicatorsDto TechnicalIndicators { get; set; } = null!;
    public string Reasoning { get; set; } = string.Empty;
    public DateTime AnalyzedAt { get; set; }
}

public class TechnicalIndicatorsDto
{
    public double CurrentPrice { get; set; }
    public double Rsi { get; set; }
    public double Sma20 { get; set; }
}