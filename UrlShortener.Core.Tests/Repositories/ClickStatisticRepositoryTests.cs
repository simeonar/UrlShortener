using System.Collections.Generic;
using System.Linq;
using Xunit;
using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Tests.Repositories
{
    public class InMemoryClickStatisticRepository
    {
        private readonly List<ClickStatistic> _clicks = new();
        public void Add(ClickStatistic click) => _clicks.Add(click);
        public IEnumerable<ClickStatistic> GetAll() => _clicks;
    }

    public class ClickStatisticRepositoryTests
    {
        [Fact]
        public void Add_And_GetAll_Works()
        {
            var repo = new InMemoryClickStatisticRepository();
            repo.Add(new ClickStatistic { Country = "RU" });
            repo.Add(new ClickStatistic { Country = "US" });
            var all = repo.GetAll().ToList();
            Assert.Equal(2, all.Count);
            Assert.Contains(all, c => c.Country == "RU");
            Assert.Contains(all, c => c.Country == "US");
        }
    }
}
