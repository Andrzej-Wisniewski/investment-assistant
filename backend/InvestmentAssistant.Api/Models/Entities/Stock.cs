namespace InvestmentAssistant.Api.Models.Entities;

/// <summary>
/// Reprezentuje akcję — dane przechowywane w bazie.
/// </summary>
public class Stock
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Symbol { get; set; } = string.Empty;

    public decimal CurrentPrice { get; set; }

    public decimal OpenPrice { get; set; }

    public decimal HighPrice { get; set; }

    public decimal LowPrice { get; set; }

    public decimal PreviousClosePrice { get; set; }

    public DateTime LastUpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<StockHistory> History { get; set; } = new List<StockHistory>();

}