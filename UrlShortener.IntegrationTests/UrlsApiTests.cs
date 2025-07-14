using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace UrlShortener.IntegrationTests
{
    public class UrlsApiTests
    {
        private readonly HttpClient _client;
        public UrlsApiTests()
        {
            _client = new HttpClient { BaseAddress = new System.Uri("http://localhost:7006") };
        }

        [Fact]
        public async Task GetUrl_ByShortCode_ReturnsCorrectData()
        {
            // Arrange
            var createReq = new { originalUrl = "https://test.com" };
            var createResp = await _client.PostAsJsonAsync("/api/shorten", createReq);
            createResp.EnsureSuccessStatusCode();
            var createJson = await createResp.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(createJson);
            var shortCode = doc.RootElement.GetProperty("shortCode").GetString();

            // Act
            var getResp = await _client.GetAsync($"/api/urls/{shortCode}");
            getResp.EnsureSuccessStatusCode();
            var getJson = await getResp.Content.ReadAsStringAsync();
            Assert.Contains("https://test.com", getJson);
            Assert.Contains(shortCode, getJson);
        }

        [Fact]
        public async Task GetUrl_NonExistent_ReturnsNotFound()
        {
            var resp = await _client.GetAsync("/api/urls/doesnotexist123");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, resp.StatusCode);
        }
    }
}
