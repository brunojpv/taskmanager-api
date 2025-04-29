using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TaskManager.Api.Tests.Helpers;
using TaskManager.Application.DTOs;

namespace TaskManager.Api.Tests.Integration
{
    public class ProjectApiTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly TestWebApplicationFactory _factory;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public ProjectApiTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetProjects_ReturnsSuccessAndProjects()
        {
            // Arrange
            var userId = Guid.NewGuid();

            await _factory.AddTestProject(userId, "Test Project 1", "Description 1");
            await _factory.AddTestProject(userId, "Test Project 2", "Description 2");

            // Act
            var response = await _client.GetAsync($"/api/projects/{userId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var projects = await response.Content.ReadFromJsonAsync<List<ProjectDTO>>(_jsonOptions);

            Assert.NotNull(projects);
            Assert.Equal(2, projects.Count);
            Assert.Equal("Test Project 1", projects[0].Name);
            Assert.Equal("Test Project 2", projects[1].Name);
        }

        [Fact]
        public async Task CreateProject_WithValidData_ReturnsCreatedAndProject()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var projectDto = new ProjectCreateDTO
            {
                Name = "New Integration Test Project",
                Description = "Created during integration test",
                UserId = userId
            };

            var content = new StringContent(
                JsonSerializer.Serialize(projectDto, _jsonOptions),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/projects", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var createdProject = await response.Content.ReadFromJsonAsync<ProjectDTO>(_jsonOptions);

            Assert.NotNull(createdProject);
            Assert.Equal(projectDto.Name, createdProject.Name);
            Assert.Equal(projectDto.Description, createdProject.Description);
            Assert.NotEqual(Guid.Empty, createdProject.Id);
        }
    }
}
