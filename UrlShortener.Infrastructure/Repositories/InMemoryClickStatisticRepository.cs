using System.Threading.Tasks;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Repositories;

namespace UrlShortener.Infrastructure.Repositories
{
    /// <summary>
    /// Dummy implementation for ClickStatistic repository (replace with EF/DB logic).
    /// </summary>
    public class InMemoryClickStatisticRepository : IClickStatisticRepository
    {
        private static readonly List<ClickStatistic> _clicks = new();
        public Task AddAsync(ClickStatistic click)
        {
            _clicks.Add(click);
            return Task.CompletedTask;
        }
    }
}
