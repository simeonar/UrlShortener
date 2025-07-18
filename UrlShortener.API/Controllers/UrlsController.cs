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
        private readonly IUserRepository _userRepository;

        public UrlsController(IShortCodeGenerator shortCodeGenerator, IShortenedUrlRepository repository, IUserRepository userRepository)
        {
            _shortCodeGenerator = shortCodeGenerator;
            _repository = repository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Creates a new shortened URL.
        /// </summary>
        [HttpPost("shorten")]
        public async Task<IActionResult> Shorten([FromBody] ShortenUrlRequest request, [FromHeader(Name = "X-Api-Key")] string apiKey = null)
        {
            var entity = new ShortenedUrl
            {
                Id = Guid.NewGuid(),
                OriginalUrl = request.OriginalUrl,
                CreatedAt = DateTime.UtcNow
            };
            if (!string.IsNullOrEmpty(apiKey))
            {
                var user = _userRepository.GetByApiKey(apiKey);
                if (user != null)
                {
                    entity.OwnerUserName = user.UserName;
                }
            }
            var code = await _shortCodeGenerator.GenerateShortCodeAsync(entity);
            entity.ShortCode = code;
            await _repository.AddAsync(entity);

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

        /// <summary>
        /// Gets info about a shortened URL by short code.
        /// </summary>
        [HttpGet("{shortCode}")]
        public async Task<IActionResult> GetByShortCode(string shortCode)
        {
            var entity = await _repository.GetByShortCodeAsync(shortCode);
            if (entity == null)
                return NotFound(new { message = "Short code not found" });

            // If link has an owner and is not public, only owner can access
            if (!string.IsNullOrEmpty(entity.OwnerUserName) && !entity.IsPublic)
            {
                var userName = User?.Identity?.IsAuthenticated == true ? User.Identity.Name : null;
                if (entity.OwnerUserName == userName)
                {
                    return Ok(BuildUrlResponse(entity));
                }
                return Forbid();
            }

            // If no owner or public, allow anyone
            return Ok(BuildUrlResponse(entity));
        }

        private object BuildUrlResponse(ShortenedUrl entity)
        {
            var requestScheme = Request.Scheme;
            var requestHost = Request.Headers["X-Forwarded-Host"].FirstOrDefault()
                ?? Request.Host.Host;
            var port = Request.Host.Port;
            if (!string.IsNullOrEmpty(port?.ToString()) && port != 80 && port != 443)
            {
                requestHost += $":{port}";
            }
            var shortUrl = $"{requestScheme}://{requestHost}/{entity.ShortCode}";
            return new
            {
                shortCode = entity.ShortCode,
                shortUrl,
                originalUrl = entity.OriginalUrl,
                createdAt = entity.CreatedAt,
                expirationDate = entity.ExpirationDate,
                clicksCount = entity.ClicksCount,
                active = entity.ExpirationDate == null || entity.ExpirationDate > DateTime.UtcNow
            };
        }
    }

    public class ShortenUrlRequest
    {
        public string OriginalUrl { get; set; } = string.Empty;
        public string? CustomAlias { get; set; }
    }
}
