using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using UrlShortener.Core.Services;

namespace UrlShortener.Infrastructure.Services
{
    public class InMemoryQRCodeCache : IQRCodeCache
    {
        private readonly ConcurrentDictionary<string, byte[]> _cache = new();

        public Task<byte[]> GetOrAddAsync(string key, Func<Task<byte[]>> valueFactory)
        {
            if (_cache.TryGetValue(key, out var value))
                return Task.FromResult(value);
            return AddAsync(key, valueFactory);
        }

        private async Task<byte[]> AddAsync(string key, Func<Task<byte[]>> valueFactory)
        {
            var value = await valueFactory();
            _cache.TryAdd(key, value);
            return value;
        }
    }
}
