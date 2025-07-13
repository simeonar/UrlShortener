using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Repositories;

namespace UrlShortener.API.Controllers
{
    [ApiController]
    [Route("api/urls")]
    /// <summary>
    /// API controller for querying shortened URLs.
    /// </summary>
    public class UrlsQueryController : ControllerBase
    {
        private readonly IShortenedUrlRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlsQueryController"/> class.
        /// </summary>
        /// <param name="repository">Shortened URL repository.</param>
        public UrlsQueryController(IShortenedUrlRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets a shortened URL by its short code, respecting privacy.
        /// Public links are available to all, private only to owner.
        /// </summary>
        /// <param name="shortCode">Short code to look up.</param>
        /// <returns>ShortenedUrl entity if allowed, otherwise 404 or 403.</returns>
        [HttpGet("find/{shortCode}")]
        public async Task<IActionResult> FindByShortCode(string shortCode)
        {
            var url = await this.repository.GetByShortCodeAsync(shortCode);
            if (url == null || url.IsBlocked)
            {
                return this.NotFound();
            }

            // If public, show to anyone
            if (url.IsPublic)
            {
                return this.Ok(url);
            }

            // If not public, only owner can see
            var userName = this.User?.Identity?.IsAuthenticated == true ? this.User.Identity.Name : null;
            if (!string.IsNullOrEmpty(url.OwnerUserName) && url.OwnerUserName == userName)
            {
                return this.Ok(url);
            }

            // Otherwise forbidden
            return this.Forbid();
        }
    }
}
