namespace InvestmentAssistant.Api.Models.Entities;

/// <summary>
/// Historia dziennych cen dla wskaźników technicznych
/// </summary>
public class StockHistory
{
    public Guid Id { get; set; }
    public Guid StockId { get; set; }
    public Stock Stock { get; set; } = null!;

    public decimal ClosePrice { get; set; } 
    public decimal OpenPrice { get; set; }
    public decimal HighPrice { get; set; }
    public decimal LowPrice { get; set; }
    public long Volume { get; set; }

    public DateTime Date { get; set; }  
    public DateTime CreatedAt { get; set; }
}