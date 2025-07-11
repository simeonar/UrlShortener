using System;

namespace UrlShortener.Core.Entities
{
    /// <summary>
    /// Represents a click event for a shortened URL.
    /// </summary>
    public class ClickStatistic
    {
        public Guid Id { get; set; }
        public Guid ShortenedUrlId { get; set; }
        public DateTime ClickedAt { get; set; }
        public string? Referrer { get; set; }
        public string? UserAgent { get; set; }
        public string? Country { get; set; }
        public string? Browser { get; set; }
        public string? IpAddress { get; set; }
        public virtual ShortenedUrl? ShortenedUrl { get; set; }
    }
}
