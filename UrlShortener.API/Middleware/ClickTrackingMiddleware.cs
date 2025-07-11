using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using UrlShortener.Core.Repositories;
using UrlShortener.Core.Entities;

namespace UrlShortener.API.Middleware
{
    /// <summary>
    /// Middleware for tracking clicks on short URLs.
    /// </summary>
    public class ClickTrackingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IShortenedUrlRepository _repository;
        private readonly IClickStatisticRepository _clickRepo;

        public ClickTrackingMiddleware(RequestDelegate next, IShortenedUrlRepository repository, IClickStatisticRepository clickRepo)
        {
            _next = next;
            _repository = repository;
            _clickRepo = clickRepo;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Only track GET /{shortCode} requests
            var path = context.Request.Path.Value;
            if (!string.IsNullOrEmpty(path) && path.Length > 1 && !path.StartsWith("/api"))
            {
                var shortCode = path.Trim('/');
                // TODO: Get ShortenedUrl by code, if exists
                var exists = await _repository.ExistsByShortCodeAsync(shortCode);
                if (exists)
                {
                    var click = new ClickStatistic
                    {
                        Id = Guid.NewGuid(),
                        ShortenedUrlId = Guid.Empty, // TODO: set real ID
                        ClickedAt = DateTime.UtcNow,
                        Referrer = context.Request.Headers["Referer"].ToString(),
                        UserAgent = context.Request.Headers["User-Agent"].ToString(),
                        IpAddress = context.Connection.RemoteIpAddress?.ToString()
                    };
                    await _clickRepo.AddAsync(click);
                }
            }
            await _next(context);
        }
    }
}
