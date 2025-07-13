using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace UrlShortener.IntegrationTests
{
    public class UserLinksApiTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private string _token;

        public UserLinksApiTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        private async Task AuthenticateAsync()
        {
            var email = "userlinks@example.com";
            var password = "Test123!@#";
            await _client.PostAsJsonAsync("/api/auth/register", new { email, password });
            var loginResp = await _client.PostAsJsonAsync("/api/auth/login", new { email, password });
            var json = await loginResp.Content.ReadAsStringAsync();
            var token = System.Text.Json.JsonDocument.Parse(json).RootElement.GetProperty("token").GetString();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _token = token;
        }

        [Fact]
        public async Task GetLinks_Unauthorized_Returns401()
        {
            var response = await _client.GetAsync("/api/user/links");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CreateAndGetLinks_Success()
        {
            await AuthenticateAsync();
            var createReq = new { url = "https://example.com/1" };
            var createResp = await _client.PostAsJsonAsync("/api/user/links", createReq);
            Assert.Equal(HttpStatusCode.OK, createResp.StatusCode);
            var listResp = await _client.GetAsync("/api/user/links");
            Assert.Equal(HttpStatusCode.OK, listResp.StatusCode);
            var json = await listResp.Content.ReadAsStringAsync();
            Assert.Contains("example.com/1", json);
        }

        [Fact]
        public async Task UpdateLink_Success()
        {
            await AuthenticateAsync();
            var createReq = new { url = "https://example.com/2" };
            var createResp = await _client.PostAsJsonAsync("/api/user/links", createReq);
            var created = await createResp.Content.ReadFromJsonAsync<dynamic>();
            string shortCode = created.shortCode;
            var updateReq = new { url = "https://example.com/2-updated" };
            var updateResp = await _client.PutAsJsonAsync($"/api/user/links/{shortCode}", updateReq);
            Assert.Equal(HttpStatusCode.OK, updateResp.StatusCode);
            var getResp = await _client.GetAsync("/api/user/links");
            var json = await getResp.Content.ReadAsStringAsync();
            Assert.Contains("example.com/2-updated", json);
        }

        [Fact]
        public async Task DeleteLink_Success()
        {
            await AuthenticateAsync();
            var createReq = new { url = "https://example.com/3" };
            var createResp = await _client.PostAsJsonAsync("/api/user/links", createReq);
            var created = await createResp.Content.ReadFromJsonAsync<dynamic>();
            string shortCode = created.shortCode;
            var delResp = await _client.DeleteAsync($"/api/user/links/{shortCode}");
            Assert.Equal(HttpStatusCode.OK, delResp.StatusCode);
            var getResp = await _client.GetAsync("/api/user/links");
            var json = await getResp.Content.ReadAsStringAsync();
            Assert.DoesNotContain("example.com/3", json);
        }
    }
}
