using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TaskManager.Application.DTOs.User;

namespace TaskManager.IntegrationTests
{
    public class AuthIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Register_And_Login_Should_Return_Jwt_Token()
        {
            var register = new
            {
                name = "Integration User",
                email = "integration@test.com",
                password = "123456"
            };

            var response = await _client.PostAsJsonAsync("/api/auth/register", register);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<AuthDto>();
            Assert.False(string.IsNullOrWhiteSpace(result!.Token));

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);
            var authTest = await _client.GetAsync("/api/projects");
            Assert.Equal(HttpStatusCode.OK, authTest.StatusCode);
        }
    }
}
