using System.Collections.Concurrent;

namespace Mango.Services.CouponAPI.Cache;

public class InMemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private static readonly ConcurrentDictionary<string, object> _cacheKeys = new ConcurrentDictionary<string, object>();

    public InMemoryCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        if (_memoryCache.TryGetValue(key, out T value))
        {
            return Task.FromResult(value);
        }

        return Task.FromResult<T?>(default);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        _memoryCache.Remove(key);               
        _cacheKeys.TryRemove(key, out _);        
        return Task.CompletedTask;               
    }

    public Task SetAsync<T>(string key, T data, int? expirationInMinutes = 5, CancellationToken cancellationToken = default)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expirationInMinutes ?? 5)
        };

        _memoryCache.Set(key, data, cacheEntryOptions);
        _cacheKeys.TryAdd(key, null);
        return Task.CompletedTask;
    }

    public IEnumerable<string> GetKeysStartingWith(string prefix)
    {
        return _cacheKeys.Keys.Where(k => k.StartsWith(prefix));
    }

    public async Task RemoveKeysStartingWith(string prefix, CancellationToken cancellationToken = default)
    {
        var keysToRemove = GetKeysStartingWith(prefix);
        foreach (var key in keysToRemove)
        {
            await RemoveAsync(key, cancellationToken);
        }
    }
}
