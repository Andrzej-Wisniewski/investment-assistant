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

    public RedisCacheService(IConnectionMultiplexer redis, ILogger<RedisCacheService> logger)
    {
        _redis = redis;
        _db = _redis.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return default;

            var value = await _db.StringGetAsync(key);

            if (!value.HasValue)
                return default;

            try
            {
                return JsonSerializer.Deserialize<T>(value.ToString());
            }
            catch
            {
                return default;
            }


    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        if (string.IsNullOrWhiteSpace(key) || value == null)
            return;

        var serializedValue = JsonSerializer.Serialize(value);

        if (expiration.HasValue)
        {
            await _db.StringSetAsync(key, serializedValue, expiration.Value);
        }
        else
        {
            await _db.StringSetAsync(key, serializedValue);
        }
    }

    public async Task RemoveAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return;

        await _db.KeyDeleteAsync(key);
    }

    public async Task<bool> ExistsAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return false;

        return await _db.KeyExistsAsync(key);
    }
}