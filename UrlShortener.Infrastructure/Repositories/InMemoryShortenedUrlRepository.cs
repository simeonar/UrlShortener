
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

        public Task AddAsync(UrlShortener.Core.Entities.ShortenedUrl url)
        {
            _shortCodes.TryAdd(url.ShortCode, url.OriginalUrl);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(UrlShortener.Core.Entities.ShortenedUrl url)
        {
            _shortCodes[url.ShortCode] = url.OriginalUrl;
            return Task.CompletedTask;
        }
        public Task<UrlShortener.Core.Entities.ShortenedUrl?> GetByShortCodeAsync(string shortCode)
        {
            if (_shortCodes.TryGetValue(shortCode, out var originalUrl))
            {
                return Task.FromResult<UrlShortener.Core.Entities.ShortenedUrl?>(new UrlShortener.Core.Entities.ShortenedUrl
                {
                    ShortCode = shortCode,
                    OriginalUrl = originalUrl,
                    CreatedAt = DateTime.UtcNow // Нет хранения даты, ставим сейчас
                });
            }
            return Task.FromResult<UrlShortener.Core.Entities.ShortenedUrl?>(null);
        }

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
