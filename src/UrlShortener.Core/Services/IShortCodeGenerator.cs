using System.Threading.Tasks;
using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Services
{
    /// <summary>
    /// Service for generating unique short codes for URLs.
    /// </summary>
    public interface IShortCodeGenerator
    {
        /// <summary>
        /// Generates a unique short code for a given URL entity.
        /// </summary>
        /// <param name="entity">ShortenedUrl entity (may be used for ID or other info)</param>
        /// <returns>Unique short code</returns>
        Task<string> GenerateShortCodeAsync(ShortenedUrl entity);
    }
}
