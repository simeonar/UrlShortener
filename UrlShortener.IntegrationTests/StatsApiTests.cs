using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace UrlShortener.IntegrationTests
{
    public class StatsApiTests
    {
        private readonly HttpClient _client;
        public StatsApiTests()
        {
            _client = new HttpClient { BaseAddress = new System.Uri("http://localhost:7006") };
        }

        [Fact]
        public async Task GetStats_ByShortCode_ReturnsStats()
        {
            // Arrange: создаём ссылку
            var createReq = new { originalUrl = "https://stats-test.com" };
            var createResp = await _client.PostAsJsonAsync("/api/shorten", createReq);
            createResp.EnsureSuccessStatusCode();
            var createJson = await createResp.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(createJson);
            var shortCode = doc.RootElement.GetProperty("shortCode").GetString();

            // Act: получаем статистику
            var statsResp = await _client.GetAsync($"/api/stats/{shortCode}");
            statsResp.EnsureSuccessStatusCode();
            var statsJson = await statsResp.Content.ReadAsStringAsync();
            Assert.Contains(shortCode, statsJson);
            Assert.Contains("totalClicks", statsJson);
        }

        [Fact]
        public async Task GetStats_NonExistent_ReturnsNotFound()
        {
            var resp = await _client.GetAsync("/api/stats/doesnotexist123");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, resp.StatusCode);
        }
    }
}
