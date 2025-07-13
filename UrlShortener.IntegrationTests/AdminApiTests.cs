using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace UrlShortener.IntegrationTests
{
    public class AdminApiTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private string _adminToken;

        public AdminApiTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        private async Task AuthenticateAdminAsync()
        {
            var email = "admin@example.com";
            var password = "Admin123!@#";
            // Предполагается, что регистрация admin создаёт пользователя с ролью admin
            await _client.PostAsJsonAsync("/api/auth/register", new { email, password, isAdmin = true });
            var loginResp = await _client.PostAsJsonAsync("/api/auth/login", new { email, password });
            var json = await loginResp.Content.ReadAsStringAsync();
            var token = System.Text.Json.JsonDocument.Parse(json).RootElement.GetProperty("token").GetString();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _adminToken = token;
        }

        [Fact]
        public async Task GetAllLinks_Unauthorized_Returns403()
        {
            var response = await _client.GetAsync("/api/admin/links");
            Assert.True(response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetAllLinks_AsAdmin_ReturnsOkAndList()
        {
            await AuthenticateAdminAsync();
            var response = await _client.GetAsync("/api/admin/links");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Contains("shortCode", json);
        }

        [Fact]
        public async Task DeleteLinks_AsAdmin_Success()
        {
            await AuthenticateAdminAsync();
            // Создать ссылку для удаления
            await _client.PostAsJsonAsync("/api/user/links", new { url = "https://to-delete.com" });
            // Получить список ссылок
            var listResp = await _client.GetAsync("/api/admin/links");
            var json = await listResp.Content.ReadAsStringAsync();
            var doc = System.Text.Json.JsonDocument.Parse(json);
            var firstShortCode = doc.RootElement[0].GetProperty("shortCode").GetString();
            // Удалить ссылку
            var delResp = await _client.DeleteAsync($"/api/admin/links/{firstShortCode}");
            Assert.Equal(HttpStatusCode.OK, delResp.StatusCode);
        }

        [Fact]
        public async Task GetAllUsers_AsAdmin_ReturnsOkAndList()
        {
            await AuthenticateAdminAsync();
            var response = await _client.GetAsync("/api/admin/users");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Contains("email", json);
        }
    }
}
