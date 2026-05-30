namespace InvestmentAssistant.Api.Services;

/// <summary>
/// Serwis zbierający dane akcji w tle.
/// </summary>
public interface IStockDataCollectorService
{
    /// <summary>
    /// Zbiera dane dla listy symboli.
    /// </summary>
    Task CollectStockDataAsync(IEnumerable<string> symbols);

    /// <summary>
    /// Ostatnia pomyślna aktualizacja.
    /// </summary>
    DateTime? LastSuccessfulUpdate { get; }
}