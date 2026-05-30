namespace InvestmentAssistant.Api.Models.Entities;

/// <summary>
/// Reprezentuje akcję — dane przechowywane w bazie.
/// </summary>
public class Stock
{
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Ticker symbol 
    /// </summary>
    public required string Symbol { get; set; }

    /// <summary>
    /// Aktualna cena.
    /// </summary>
    public decimal CurrentPrice { get; set; }

    /// <summary>
    /// Cena otwarcia dzisiejszego dnia
    /// </summary>
    public decimal OpenPrice { get; set; }

    /// <summary>
    /// Najwyższa cena dzisiaj
    /// </summary>
    public decimal HighPrice { get; set; }

    /// <summary>
    /// Najniższa cena dzisiaj
    /// </summary>
    public decimal LowPrice { get; set; }

    /// <summary>
    /// Ostatnia cena zamknięcia (z poprzedniego dnia)
    /// </summary>
    public decimal PreviousClosePrice { get; set; }

    /// <summary>
    /// Kiedy ostatnio pobrano dane z API
    /// </summary>
    public DateTime LastUpdatedAt { get; set; }

    /// <summary>
    /// Kiedy rekord został dodany do bazy
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}