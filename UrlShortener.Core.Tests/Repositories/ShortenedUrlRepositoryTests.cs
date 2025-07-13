using System.Collections.Generic;
using System.Linq;
using Xunit;
using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Tests.Repositories
{
    public class InMemoryShortenedUrlRepository
    {
        private readonly List<ShortenedUrl> _urls = new();
        public void Add(ShortenedUrl url) => _urls.Add(url);
        public ShortenedUrl? GetByShortCode(string code) => _urls.FirstOrDefault(u => u.ShortCode == code);
        public IEnumerable<ShortenedUrl> GetAll() => _urls;
        public void DeleteByShortCode(string code)
        {
            var url = _urls.FirstOrDefault(u => u.ShortCode == code);
            if (url != null) _urls.Remove(url);
        }
    }

    public class ShortenedUrlRepositoryTests
    {
        [Fact]
        public void Add_And_GetByShortCode_Works()
        {
            var repo = new InMemoryShortenedUrlRepository();
            var url = new ShortenedUrl { ShortCode = "abc" };
            repo.Add(url);
            var found = repo.GetByShortCode("abc");
            Assert.NotNull(found);
        }

        [Fact]
        public void DeleteByShortCode_RemovesUrl()
        {
            var repo = new InMemoryShortenedUrlRepository();
            repo.Add(new ShortenedUrl { ShortCode = "abc" });
            repo.DeleteByShortCode("abc");
            Assert.Null(repo.GetByShortCode("abc"));
        }

        [Fact]
        public void GetAll_ReturnsAllUrls()
        {
            var repo = new InMemoryShortenedUrlRepository();
            repo.Add(new ShortenedUrl { ShortCode = "a" });
            repo.Add(new ShortenedUrl { ShortCode = "b" });
            var all = repo.GetAll().ToList();
            Assert.Equal(2, all.Count);
        }
    }
}
