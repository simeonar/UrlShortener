using System;

namespace UrlShortener.Core.Entities
{
    /// <summary>
    /// Represents a shortened URL and its metadata.
    /// </summary>
    public class ShortenedUrl
    {
        public Guid Id { get; set; }
        public string OriginalUrl { get; set; } = string.Empty;
        public string ShortCode { get; set; } = string.Empty;
        public string? CustomAlias { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int ClicksCount { get; set; }
        public virtual ICollection<ClickStatistic> ClickStatistics { get; set; } = new List<ClickStatistic>();
        public string? OwnerUserName { get; set; } // For user dashboard
        public bool IsBlocked { get; set; } = false; // For moderation
        public bool IsPublic { get; set; } = false; // If true, link is visible to anyone by code
    }
}
