namespace InvestmentAssistant.Api.Infrastructure.Cache
{
    /// <summary>
    /// Interfejs serwisu cache
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Pobiera wartość z cache'u po kluczu
        /// </summary>
        Task<T?> GetAsync<T>(string key);

        /// <summary>
        /// Ustawia wartośc w cach'u z opcjonalnym czasem wygaśnięcia
        /// </summary>
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);

        /// <summary>
        /// usuwa wartość z cache'u po kluczu
        /// </summary>
        Task RemoveAsync(string key);

        /// <summary>
        /// Sprawdza, czy klucz istnieje w cache'u
        /// </summary>
        Task<bool> ExistsAsync(string key);
    }
}