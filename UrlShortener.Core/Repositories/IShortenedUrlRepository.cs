using System.Threading.Tasks;

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
    }
}
