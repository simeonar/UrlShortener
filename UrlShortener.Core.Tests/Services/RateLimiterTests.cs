using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Xunit;

namespace UrlShortener.Core.Tests.Services
{
    public class InMemoryRateLimiter
    {
        private readonly ConcurrentDictionary<string, (int Count, DateTime WindowStart)> _requests = new();
        private readonly int _limit;
        private readonly TimeSpan _window;
        public InMemoryRateLimiter(int limit, TimeSpan window)
        {
            _limit = limit;
            _window = window;
        }
        public bool AllowRequest(string key)
        {
            var now = DateTime.UtcNow;
            var entry = _requests.GetOrAdd(key, _ => (0, now));
            if (now - entry.WindowStart > _window)
            {
                _requests[key] = (1, now);
                return true;
            }
            if (entry.Count < _limit)
            {
                _requests[key] = (entry.Count + 1, entry.WindowStart);
                return true;
            }
            return false;
        }
    }

    public class RateLimiterTests
    {
        [Fact]
        public void AllowRequest_RespectsLimitPerKey()
        {
            var limiter = new InMemoryRateLimiter(3, TimeSpan.FromMinutes(1));
            Assert.True(limiter.AllowRequest("ip1"));
            Assert.True(limiter.AllowRequest("ip1"));
            Assert.True(limiter.AllowRequest("ip1"));
            Assert.False(limiter.AllowRequest("ip1"));
            Assert.True(limiter.AllowRequest("ip2")); // другой ключ
        }
    }
}
