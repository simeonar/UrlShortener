using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System;

namespace UrlShortener.API.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ConcurrentDictionary<string, (int Count, DateTime WindowStart)> _requests = new();
        private readonly int _guestLimit = 30; // requests per window
        private readonly int _userLimit = 100; // requests per window
        private readonly TimeSpan _window = TimeSpan.FromMinutes(1);

        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string key;
            bool isUser = false;
            if (context.Request.Headers.TryGetValue("X-Api-Key", out var apiKey) && !string.IsNullOrEmpty(apiKey))
            {
                key = $"user:{apiKey}";
                isUser = true;
            }
            else
            {
                key = $"ip:{context.Connection.RemoteIpAddress}";
            }

            var now = DateTime.UtcNow;
            var (count, windowStart) = _requests.GetOrAdd(key, _ => (0, now));
            if (now - windowStart > _window)
            {
                // Reset window
                _requests[key] = (1, now);
            }
            else
            {
                int limit = isUser ? _userLimit : _guestLimit;
                if (count >= limit)
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.Response.Headers["Retry-After"] = _window.TotalSeconds.ToString();
                    await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
                    return;
                }
                _requests[key] = (count + 1, windowStart);
            }

            await _next(context);
        }
    }
}
