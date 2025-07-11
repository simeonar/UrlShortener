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
    }
}
