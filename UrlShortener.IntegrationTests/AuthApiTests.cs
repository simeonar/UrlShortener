using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace UrlShortener.IntegrationTests
{
    public class AuthApiTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AuthApiTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Register_Success_ReturnsOkAndUserData()
        {
            var request = new { email = "testuser1@example.com", password = "Test123!@#" };
            var response = await _client.PostAsJsonAsync("/api/auth/register", request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Contains("testuser1@example.com", json);
        }

        [Fact]
        public async Task Register_ExistingEmail_ReturnsBadRequest()
        {
            var request = new { email = "testuser2@example.com", password = "Test123!@#" };
            // First registration
            await _client.PostAsJsonAsync("/api/auth/register", request);
            // Second registration with same email
            var response = await _client.PostAsJsonAsync("/api/auth/register", request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_Success_ReturnsOkAndToken()
        {
            var register = new { email = "testuser3@example.com", password = "Test123!@#" };
            await _client.PostAsJsonAsync("/api/auth/register", register);
            var login = new { email = "testuser3@example.com", password = "Test123!@#" };
            var response = await _client.PostAsJsonAsync("/api/auth/login", login);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Contains("token", json, System.StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task Login_InvalidPassword_ReturnsUnauthorized()
        {
            var register = new { email = "testuser4@example.com", password = "Test123!@#" };
            await _client.PostAsJsonAsync("/api/auth/register", register);
            var login = new { email = "testuser4@example.com", password = "WrongPassword" };
            var response = await _client.PostAsJsonAsync("/api/auth/login", login);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
