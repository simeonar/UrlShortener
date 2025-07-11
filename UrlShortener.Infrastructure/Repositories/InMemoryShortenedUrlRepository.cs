using System.Collections.Concurrent;
using System.Threading.Tasks;
using UrlShortener.Core.Repositories;

namespace UrlShortener.Infrastructure.Repositories
{
    /// <summary>
    /// In-memory implementation of IShortenedUrlRepository for development and testing.
    /// </summary>
    public class InMemoryShortenedUrlRepository : IShortenedUrlRepository
    {
        private readonly ConcurrentDictionary<string, string> _shortCodes = new();

        public Task<bool> ExistsByShortCodeAsync(string shortCode)
        {
            return Task.FromResult(_shortCodes.ContainsKey(shortCode));
        }

        public Task<string?> GetOriginalUrlByShortCodeAsync(string shortCode)
        {
            _shortCodes.TryGetValue(shortCode, out var originalUrl);
            return Task.FromResult(originalUrl);
        }

        // For testing/demo: add a short code and its original URL
        public Task AddShortCodeAsync(string shortCode, string originalUrl)
        {
            _shortCodes.TryAdd(shortCode, originalUrl);
            return Task.CompletedTask;
        }
    }
}
