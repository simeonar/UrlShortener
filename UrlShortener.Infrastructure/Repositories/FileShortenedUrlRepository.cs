using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Repositories;

namespace UrlShortener.Infrastructure.Repositories
{
    public class FileShortenedUrlRepository : IShortenedUrlRepository
    {
        public async Task DeleteByShortCodeAsync(string shortCode)
        {
            lock (_lock)
            {
                var all = GetAllAsync().Result;
                var idx = all.FindIndex(x => x.ShortCode == shortCode);
                if (idx >= 0)
                {
                    all.RemoveAt(idx);
                    File.WriteAllText(_filePath, JsonSerializer.Serialize(all));
                }
            }
        }
        // ...existing code...

        public async Task<bool> ExistsByShortCodeAsync(string shortCode)
        {
            var all = await GetAllAsync();
            return all.Exists(x => x.ShortCode == shortCode);
        }

        public async Task<string?> GetOriginalUrlByShortCodeAsync(string shortCode)
        {
            var all = await GetAllAsync();
            var found = all.Find(x => x.ShortCode == shortCode);
            return found?.OriginalUrl;
        }
    
        private readonly string _filePath;
        private readonly object _lock = new();

        public FileShortenedUrlRepository(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<List<ShortenedUrl>> GetAllAsync()
        {
            lock (_lock)
            {
                if (!File.Exists(_filePath))
                    return new List<ShortenedUrl>();
                var json = File.ReadAllText(_filePath);
                var list = JsonSerializer.Deserialize<List<ShortenedUrl>>(json) ?? new List<ShortenedUrl>();
                // Ensure IsPublic is set for legacy records
                foreach (var url in list)
                {
                    if (url.IsPublic != true && url.IsPublic != false)
                        url.IsPublic = false;
                }
                return list;
            }
        }

        public async Task<ShortenedUrl?> GetByShortCodeAsync(string shortCode)
        {
            var all = await GetAllAsync();
            return all.Find(x => x.ShortCode == shortCode);
        }

        public async Task AddAsync(ShortenedUrl url)
        {
            lock (_lock)
            {
                var all = GetAllAsync().Result;
                if (url.IsPublic != true && url.IsPublic != false)
                    url.IsPublic = false;
                all.Add(url);
                File.WriteAllText(_filePath, JsonSerializer.Serialize(all));
            }
        }

        public async Task UpdateAsync(ShortenedUrl url)
        {
            lock (_lock)
            {
                var all = GetAllAsync().Result;
                var idx = all.FindIndex(x => x.ShortCode == url.ShortCode);
                if (idx >= 0)
                {
                    all[idx] = url;
                    File.WriteAllText(_filePath, JsonSerializer.Serialize(all));
                }
            }
        }
        public async Task DeleteAsync(Guid id)
        {
            lock (_lock)
            {
                var all = GetAllAsync().Result;
                var idx = all.FindIndex(x => x.Id == id);
                if (idx >= 0)
                {
                    all.RemoveAt(idx);
                    File.WriteAllText(_filePath, JsonSerializer.Serialize(all));
                }
            }
        }
    }
}
