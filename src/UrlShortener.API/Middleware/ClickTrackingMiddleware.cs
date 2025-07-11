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
        // TODO: Inject service for persisting ClickStatistic

        public ClickTrackingMiddleware(RequestDelegate next, IShortenedUrlRepository repository)
        {
            _next = next;
            _repository = repository;
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
                    // Collect click data
                    var click = new ClickStatistic
                    {
                        Id = Guid.NewGuid(),
                        ShortenedUrlId = Guid.Empty, // TODO: set real ID
                        ClickedAt = DateTime.UtcNow,
                        Referrer = context.Request.Headers["Referer"].ToString(),
                        UserAgent = context.Request.Headers["User-Agent"].ToString(),
                        IpAddress = context.Connection.RemoteIpAddress?.ToString()
                        // TODO: Country, Browser detection
                    };
                    // TODO: Save click to DB
                }
            }
            await _next(context);
        }
    }
}
