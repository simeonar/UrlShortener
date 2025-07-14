using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace UrlShortener.IntegrationTests
{
    public class RedirectApiTests
    {
        private readonly HttpClient _client;
        public RedirectApiTests()
        {
            _client = new HttpClient { BaseAddress = new System.Uri("http://localhost:7006") };
            _client.Timeout = System.TimeSpan.FromSeconds(10);
        }

        [Fact]
        public async Task Redirect_ByShortCode_Returns302AndLocation()
        {
            // Arrange
            var createReq = new { originalUrl = "https://redirect-test.com" };
            var createResp = await _client.PostAsJsonAsync("/api/shorten", createReq);
            createResp.EnsureSuccessStatusCode();
            var createJson = await createResp.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(createJson);
            var shortCode = doc.RootElement.GetProperty("shortCode").GetString();

            // Act
            var handler = new HttpClientHandler { AllowAutoRedirect = false };
            using var noRedirectClient = new HttpClient(handler) { BaseAddress = _client.BaseAddress };
            var resp = await noRedirectClient.GetAsync($"/{shortCode}");
            Assert.Equal(HttpStatusCode.Redirect, resp.StatusCode);
            Assert.Equal("https://redirect-test.com", resp.Headers.Location?.ToString());
        }

        [Fact]
        public async Task Redirect_NonExistent_ReturnsNotFound()
        {
            var handler = new HttpClientHandler { AllowAutoRedirect = false };
            using var noRedirectClient = new HttpClient(handler) { BaseAddress = _client.BaseAddress };
            var resp = await noRedirectClient.GetAsync("/notfound123");
            Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
        }
    }
}
