using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UrlShortener.Core.Repositories;

namespace UrlShortener.API.Controllers
{
    [ApiController]
    [Route("api/stats")]
    public class StatsController : ControllerBase
    {

        private readonly IClickStatisticRepository _clickRepo;
        private readonly IShortenedUrlRepository _urlRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatsController"/> class.
        /// </summary>
        /// <param name="clickRepo">The click statistics repository.</param>
        /// <param name="urlRepo">The shortened URL repository.</param>
        public StatsController(IClickStatisticRepository clickRepo, IShortenedUrlRepository urlRepo)
        {
            _clickRepo = clickRepo;
            _urlRepo = urlRepo;
        }

        /// <summary>
        /// Returns general statistics for a short code (mock implementation).
        /// </summary>
        [HttpGet("{shortCode}")]
        public async Task<IActionResult> GetStats(string shortCode)
        {
            var entity = await _urlRepo.GetByShortCodeAsync(shortCode);
            if (entity == null)
                return NotFound();

            var byDay = new[]
            {
                new { date = entity.CreatedAt.ToString("yyyy-MM-dd"), count = entity.ClicksCount }
            };

            var stats = new
            {
                shortCode = entity.ShortCode,
                totalClicks = entity.ClicksCount,
                lastClick = (string?)null, 
                byDay
            };
            return Ok(stats);
        }

        /// <summary>
        /// Returns all click events for a short code (mock implementation).
        /// </summary>
        [HttpGet("{shortCode}/clicks")]
        public async Task<IActionResult> GetClicks(string shortCode)
        {
            // TODO: Replace with real click list
            var clicks = new[]
            {
                new { clickedAt = "2025-07-11T12:00:00Z", referrer = "https://google.com", userAgent = "Mozilla/5.0" },
                new { clickedAt = "2025-07-11T13:00:00Z", referrer = "", userAgent = "Mozilla/5.0" }
            };
            return Ok(clicks);
        }

        /// <summary>
        /// Returns chart data for a short code (mock implementation).
        /// </summary>
        [HttpGet("{shortCode}/chart")]
        public async Task<IActionResult> GetChart(string shortCode)
        {
            // TODO: Replace with real chart data
            var chart = new[]
            {
                new { date = "2025-07-10", clicks = 5 },
                new { date = "2025-07-11", clicks = 7 }
            };
            return Ok(chart);
        }
        /// <summary>
        /// Returns grouped clicks by day and hour (mock).
        /// </summary>
        [HttpGet("{shortCode}/grouped")]
        public async Task<IActionResult> GetGrouped(string shortCode)
        {
            var grouped = new[]
            {
                new { date = "2025-07-10", hour = 12, clicks = 3 },
                new { date = "2025-07-10", hour = 13, clicks = 2 },
                new { date = "2025-07-11", hour = 9, clicks = 5 }
            };
            return Ok(grouped);
        }

        /// <summary>
        /// Returns top countries and browsers (mock).
        /// </summary>
        [HttpGet("{shortCode}/top")]
        public async Task<IActionResult> GetTop(string shortCode)
        {
            var top = new
            {
                countries = new[] { new { country = "RU", clicks = 5 }, new { country = "US", clicks = 3 } },
                browsers = new[] { new { browser = "Chrome", clicks = 6 }, new { browser = "Firefox", clicks = 2 } }
            };
            return Ok(top);
        }

        /// <summary>
        /// Returns traffic sources analytics (mock).
        /// </summary>
        [HttpGet("{shortCode}/sources")]
        public async Task<IActionResult> GetSources(string shortCode)
        {
            var sources = new[]
            {
                new { source = "google.com", clicks = 4 },
                new { source = "yandex.ru", clicks = 2 },
                new { source = "direct", clicks = 3 },
            };
            return Ok(sources);
        }
    }
}
