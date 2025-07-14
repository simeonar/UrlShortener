using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace UrlShortener.IntegrationTests
{
    public class ShortenApiTests
    {
        private readonly HttpClient _client;
        public ShortenApiTests()
        {
            _client = new HttpClient { BaseAddress = new System.Uri("http://localhost:7006") };
        }

        [Fact]
        public async Task Shorten_ValidUrl_ReturnsShortCode()
        {
            var request = new { originalUrl = "https://example.com" };
            var response = await _client.PostAsJsonAsync("/api/shorten", request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            Assert.Contains("shortCode", json);
        }

        [Fact]
        public async Task Shorten_InvalidUrl_ReturnsBadRequest()
        {
            var request = new { originalUrl = "not_a_url" };
            var response = await _client.PostAsJsonAsync("/api/shorten", request);
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
