
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Repositories;

namespace UrlShortener.Infrastructure.Repositories
{
    /// <summary>
    /// In-memory implementation of IShortenedUrlRepository for development and testing.
    /// </summary>
    public class InMemoryShortenedUrlRepository : IShortenedUrlRepository
    {
        private readonly ConcurrentDictionary<string, ShortenedUrl> _shortCodes = new();

        public Task<bool> ExistsByShortCodeAsync(string shortCode)
        {
            return Task.FromResult(_shortCodes.ContainsKey(shortCode));
        }

        public Task<string?> GetOriginalUrlByShortCodeAsync(string shortCode)
        {
            if (_shortCodes.TryGetValue(shortCode, out var url))
                return Task.FromResult<string?>(url.OriginalUrl);
            return Task.FromResult<string?>(null);
        }

        public Task<ShortenedUrl?> GetByShortCodeAsync(string shortCode)
        {
            if (_shortCodes.TryGetValue(shortCode, out var url))
                return Task.FromResult<ShortenedUrl?>(url);
            return Task.FromResult<ShortenedUrl?>(null);
        }

        public Task AddAsync(ShortenedUrl url)
        {
            _shortCodes.TryAdd(url.ShortCode, url);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(ShortenedUrl url)
        {
            _shortCodes[url.ShortCode] = url;
            return Task.CompletedTask;
        }

        public Task<List<ShortenedUrl>> GetAllAsync()
        {
            return Task.FromResult(_shortCodes.Values.ToList());
        }
        public Task DeleteAsync(Guid id)
        {
            var item = _shortCodes.Values.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                _shortCodes.TryRemove(item.ShortCode, out _);
            }
            return Task.CompletedTask;
        }
    }
}

