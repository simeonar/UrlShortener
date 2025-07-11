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
        private readonly ConcurrentDictionary<string, bool> _shortCodes = new();

        public Task<bool> ExistsByShortCodeAsync(string shortCode)
        {
            return Task.FromResult(_shortCodes.ContainsKey(shortCode));
        }

        // For testing/demo: add a short code
        public Task AddShortCodeAsync(string shortCode)
        {
            _shortCodes.TryAdd(shortCode, true);
            return Task.CompletedTask;
        }
    }
}
