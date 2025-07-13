using System.Threading.Tasks;
using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Repositories
{
    /// <summary>
    /// Repository abstraction for ShortenedUrl entity.
    /// </summary>
    public interface IShortenedUrlRepository
    {
        /// <summary>
        /// Checks if a short code already exists.
        /// </summary>
        /// <param name="shortCode">Short code to check</param>
        /// <returns>True if exists, otherwise false</returns>
        Task<bool> ExistsByShortCodeAsync(string shortCode);

        /// <summary>
        /// Gets the original URL by short code.
        /// </summary>
        /// <param name="shortCode">Short code to look up</param>
        /// <returns>Original URL if found, otherwise null</returns>
        Task<string?> GetOriginalUrlByShortCodeAsync(string shortCode);

        /// <summary>
        /// Gets the full ShortenedUrl entity by short code.
        /// </summary>
        /// <param name="shortCode">Short code to look up</param>
        /// <returns>ShortenedUrl entity if found, otherwise null</returns>
        Task<ShortenedUrl?> GetByShortCodeAsync(string shortCode);

        /// <summary>
        /// Adds a new ShortenedUrl entity.
        /// </summary>
        /// <param name="url">ShortenedUrl entity to add</param>
        Task AddAsync(ShortenedUrl url);

        /// <summary>
        /// Updates an existing ShortenedUrl entity.
        /// </summary>
        /// <param name="url">ShortenedUrl entity to update</param>
        Task UpdateAsync(ShortenedUrl url);
        /// <summary>
        /// Gets all shortened URLs.
        /// </summary>
        /// <returns>List of all ShortenedUrl entities</returns>
        Task<List<ShortenedUrl>> GetAllAsync();
    }
}
