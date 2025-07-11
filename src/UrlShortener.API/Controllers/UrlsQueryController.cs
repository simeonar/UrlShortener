using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Repositories;

namespace UrlShortener.API.Controllers
{
    [ApiController]
    [Route("api/urls")]
    public class UrlsQueryController : ControllerBase
    {
        private readonly IShortenedUrlRepository _repository;

        public UrlsQueryController(IShortenedUrlRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets info about a shortened URL by its short code.
        /// </summary>
        [HttpGet("{shortCode}")]
        public async Task<IActionResult> GetByShortCode(string shortCode)
        {
            // TODO: Replace with real DB call
            var exists = await _repository.ExistsByShortCodeAsync(shortCode);
            if (!exists)
                return NotFound();
            // TODO: Return full entity info
            return Ok(new { shortCode });
        }
    }
}
