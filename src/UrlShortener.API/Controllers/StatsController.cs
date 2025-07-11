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

        public StatsController(IClickStatisticRepository clickRepo)
        {
            _clickRepo = clickRepo;
        }

        /// <summary>
        /// Returns general statistics for a short code (mock implementation).
        /// </summary>
        [HttpGet("{shortCode}")]
        public async Task<IActionResult> GetStats(string shortCode)
        {
            // TODO: Replace with real aggregation logic
            var stats = new
            {
                shortCode,
                totalClicks = 42,
                lastClick = "2025-07-11T12:00:00Z"
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
    }
}
