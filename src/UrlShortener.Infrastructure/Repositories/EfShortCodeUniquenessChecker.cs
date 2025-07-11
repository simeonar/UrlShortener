using System.Threading.Tasks;
using UrlShortener.Core.Repositories;
using UrlShortener.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IShortCodeUniquenessChecker using the repository.
    /// </summary>
    public class EfShortCodeUniquenessChecker : IShortCodeUniquenessChecker
    {
        private readonly IShortenedUrlRepository _repository;
        public EfShortCodeUniquenessChecker(IShortenedUrlRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> IsShortCodeUniqueAsync(string code)
        {
            return !await _repository.ExistsByShortCodeAsync(code);
        }
    }
}
