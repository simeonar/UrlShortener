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
            var originalUrl = await _repository.GetOriginalUrlByShortCodeAsync(shortCode);
            if (string.IsNullOrEmpty(originalUrl))
                return NotFound();
            return Redirect(originalUrl);
        }
    }
}
