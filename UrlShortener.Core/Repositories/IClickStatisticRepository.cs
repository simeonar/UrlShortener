using System.Threading.Tasks;
using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Repositories
{
    /// <summary>
    /// Repository abstraction for ClickStatistic entity.
    /// </summary>
    public interface IClickStatisticRepository
    {
        Task AddAsync(ClickStatistic click);
    }
}
