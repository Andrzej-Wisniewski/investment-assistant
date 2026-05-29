using System.Text.Json;
using StackExchange.Redis;

namespace InvestmentAssistant.Api.Infrastructure.Cache;

///<summary>
/// Implementacja cache'u używająca Redisa.
///</summary>
public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _db;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _redis = redis;
        // Pobieramy bazę danych Redis (domyślnie 0)
        _db = _redis.GetDatabase();
    }

    ///<summary>
    /// Pobiera wartość z cache'u po kluczu. Deserializuje z JSON na typ T.
    /// </summary>
    public async Task<T?> GetAsync<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return default;

        var value = await _db.StringGetAsync(key);

        if (!value.HasValue)
            return default;

        try
        {
            // Deserializujemy wartość z JSONa
            return JsonSerializer.Deserialize<T>(value.ToString());
        }
        catch
        {
            // W przypadku błędu deserializacji zwracamy default
            return default;
        }
    }

    ///<summary>
    /// Ustawia wartośc w cach'u. Serializuje wartość do JSONa. 
    /// </summary>
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        if (string.IsNullOrWhiteSpace(key) || value == null)
            return;

        // Serializujemy obiekt na JSON
        var serializedValue = JsonSerializer.Serialize(value);

        // Zapisujemy do Redis'a z opcjonalnym czasem wygaśnięcia
        // Jeśli expiration jest ustawione używamy w przeciwnym razie wywołujemy wersję bez tego parametru, aby uniknąć problemów z konwersją typów.
        if (expiration.HasValue)
        {
            await _db.StringSetAsync(key, serializedValue, expiration.Value);
        }
        else
        {
            await _db.StringSetAsync(key, serializedValue);
        }
    }

    /// <summary>
    /// Usuwa klucz z cache'u 
    /// </summary>
    public async Task RemoveAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return;

        await _db.KeyDeleteAsync(key);
    }

    /// <summary>
    /// Sprawdza, czy klucz istnieje w cache'u
    /// </summary>
    public async Task<bool> ExistsAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return false;

        return await _db.KeyExistsAsync(key);
    }
}