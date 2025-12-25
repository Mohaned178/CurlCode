using CurlCode.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace CurlCode.Infrastructure.Services;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ConcurrentDictionary<string, byte> _keys = new();

    public MemoryCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public Task<T?> GetAsync<T>(string key)
    {
        _memoryCache.TryGetValue(key, out T? value);
        return Task.FromResult(value);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(10)
        };
        
        // Register post-eviction callback to remove from keys set
        options.RegisterPostEvictionCallback((k, v, r, s) => 
        {
            _keys.TryRemove(k.ToString()!, out _);
        });

        _memoryCache.Set(key, value, options);
        _keys.TryAdd(key, 0);
        
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        _memoryCache.Remove(key);
        _keys.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    public Task RemoveByPrefixAsync(string prefixKey)
    {
        var keysToRemove = _keys.Keys.Where(k => k.Contains(prefixKey)).ToList();
        foreach (var key in keysToRemove)
        {
            RemoveAsync(key);
        }
        return Task.CompletedTask;
    }
}
