using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Xunit;
using UrlShortener.Core.Services;

namespace UrlShortener.Core.Tests.Services
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
            _cache[key] = value;
            return value;
        }
        public int Count => _cache.Count;
    }

    public class QRCodeCacheTests
    {
        [Fact]
        public async Task GetOrAddAsync_CachesValue()
        {
            var cache = new InMemoryQRCodeCache();
            int factoryCalls = 0;
            Func<Task<byte[]>> factory = async () => { factoryCalls++; await Task.Yield(); return new byte[] { 1, 2, 3 }; };
            var v1 = await cache.GetOrAddAsync("key1", factory);
            var v2 = await cache.GetOrAddAsync("key1", factory);
            Assert.Equal(v1, v2);
            Assert.Equal(1, factoryCalls);
            Assert.Equal(1, cache.Count);
        }
    }
}
