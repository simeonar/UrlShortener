using System.Net.Http;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json;
using Xunit;

namespace UrlShortener.IntegrationTests
{
    public class QrApiTests
    {
        private readonly HttpClient _client;
        public QrApiTests()
        {
            _client = new HttpClient { BaseAddress = new System.Uri("http://localhost:7006") };
        }

        [Fact]
        public async Task GetQr_ByShortCode_ReturnsPng()
        {
            // Arrange
            var createReq = new { originalUrl = "https://qr-test.com" };
            var createResp = await _client.PostAsJsonAsync("/api/shorten", createReq);
            createResp.EnsureSuccessStatusCode();
            var createJson = await createResp.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(createJson);
            var shortCode = doc.RootElement.GetProperty("shortCode").GetString();

            // Act
            var qrResp = await _client.GetAsync($"/api/qr/{shortCode}");
            qrResp.EnsureSuccessStatusCode();
            Assert.Equal("image/png", qrResp.Content.Headers.ContentType?.MediaType);
            var bytes = await qrResp.Content.ReadAsByteArrayAsync();
            Assert.True(bytes.Length > 100); 
        }

        [Fact]
        public async Task GetQr_NonExistent_ReturnsNotFound()
        {
            var resp = await _client.GetAsync("/api/qr/doesnotexist123");
            Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
        }
    }
}
