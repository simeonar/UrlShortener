using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UrlShortener.Core.Repositories;

namespace UrlShortener.API.Controllers
{
    [ApiController]
    public class RedirectController : ControllerBase
    {
        private readonly IShortenedUrlRepository _repository;

        public RedirectController(IShortenedUrlRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Redirects to the original URL by short code.
        /// </summary>
        [HttpGet("/{shortCode}")]
        public async Task<IActionResult> RedirectToOriginal(string shortCode)
        {
            // TODO: Replace with real DB call to get original URL
            var exists = await _repository.ExistsByShortCodeAsync(shortCode);
            if (!exists)
                return NotFound();
            // TODO: Replace with real original URL
            var originalUrl = "https://example.com";
            return Redirect(originalUrl);
        }
    }
}
