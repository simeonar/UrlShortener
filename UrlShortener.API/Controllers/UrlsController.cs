using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Services;
using UrlShortener.Core.Repositories;

namespace UrlShortener.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlsController : ControllerBase
    {
        private readonly IShortCodeGenerator _shortCodeGenerator;
        private readonly IShortenedUrlRepository _repository;

        public UrlsController(IShortCodeGenerator shortCodeGenerator, IShortenedUrlRepository repository)
        {
            _shortCodeGenerator = shortCodeGenerator;
            _repository = repository;
        }

        /// <summary>
        /// Creates a new shortened URL.
        /// </summary>
        [HttpPost("shorten")]
        public async Task<IActionResult> Shorten([FromBody] ShortenUrlRequest request)
        {
            // TODO: Add validation, persistence, etc.
            var entity = new ShortenedUrl
            {
                Id = Guid.NewGuid(),
                OriginalUrl = request.OriginalUrl,
                CreatedAt = DateTime.UtcNow
            };
            var code = await _shortCodeGenerator.GenerateShortCodeAsync(entity);
            entity.ShortCode = code;
            // Save entity to in-memory repository
            if (_repository is UrlShortener.Infrastructure.Repositories.InMemoryShortenedUrlRepository memRepo)
            {
                await memRepo.AddShortCodeAsync(code, entity.OriginalUrl);
            }

            var requestScheme = Request.Scheme;
            var requestHost = Request.Headers["X-Forwarded-Host"].FirstOrDefault()
                ?? Request.Host.Host;
            var port = Request.Host.Port;
            if (!string.IsNullOrEmpty(port?.ToString()) && port != 80 && port != 443)
            {
                requestHost += $":{port}";
            }
            var shortUrl = $"{requestScheme}://{requestHost}/{code}";
            return Ok(new { shortCode = code, shortUrl, originalUrl = entity.OriginalUrl });
        }
    }

    public class ShortenUrlRequest
    {
        public string OriginalUrl { get; set; } = string.Empty;
        public string? CustomAlias { get; set; }
    }
}
