using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TaskManager.Api.Tests.Helpers;
using TaskManager.Application.DTOs;
using TaskManager.Application.Services;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Api.Tests.Controllers
{
    public class ProjectEndpointsTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly TestWebApplicationFactory _factory;
        private readonly Mock<IProjectService> _mockProjectService;

        public ProjectEndpointsTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _mockProjectService = new Mock<IProjectService>();
        }

        [Fact]
        public async Task GetProjectsByUserId_ReturnsOkResult_WithProjects()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var projects = new List<ProjectDTO>
            {
                new ProjectDTO
                {
                    Id = Guid.NewGuid(),
                    Name = "Project 1",
                    Description = "Description 1",
                    CreatedAt = DateTime.UtcNow,
                    TaskCount = 5
                },
                new ProjectDTO
                {
                    Id = Guid.NewGuid(),
                    Name = "Project 2",
                    Description = "Description 2",
                    CreatedAt = DateTime.UtcNow,
                    TaskCount = 3
                }
            };

            // Configurar o mock do serviço de projetos
            _mockProjectService.Setup(service => service.GetAllByUserIdAsync(userId))
                .ReturnsAsync(projects);

            // Registrar o serviço mockado
            _factory.ReplaceService(_mockProjectService.Object);

            // Act
            var response = await _client.GetAsync($"/api/projects/{userId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var returnedProjects = await response.Content.ReadFromJsonAsync<List<ProjectDTO>>();
            Assert.Equal(2, returnedProjects.Count);
            Assert.Equal("Project 1", returnedProjects[0].Name);
            Assert.Equal("Project 2", returnedProjects[1].Name);
        }

        [Fact]
        public async Task GetProjectsByUserId_ThrowsDomainException_ReturnsBadRequest()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var errorMessage = "Error retrieving projects";

            // Configurar o mock para lançar uma exceção
            _mockProjectService.Setup(service => service.GetAllByUserIdAsync(userId))
                .ThrowsAsync(new DomainException(errorMessage));

            // Registrar o serviço mockado
            _factory.ReplaceService(_mockProjectService.Object);

            // Act
            var response = await _client.GetAsync($"/api/projects/{userId}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains(errorMessage, content);
        }

        [Fact]
        public async Task CreateProject_WithValidData_ReturnsCreatedResult()
        {
            // Arrange
            var projectDto = new ProjectCreateDTO
            {
                Name = "New Project",
                Description = "Project Description",
                UserId = Guid.NewGuid()
            };

            var createdProject = new ProjectDTO
            {
                Id = Guid.NewGuid(),
                Name = projectDto.Name,
                Description = projectDto.Description,
                CreatedAt = DateTime.UtcNow,
                TaskCount = 0
            };

            // Configurar o mock para retornar o projeto criado
            _mockProjectService.Setup(service => service.CreateAsync(It.IsAny<ProjectCreateDTO>()))
                .ReturnsAsync(createdProject);

            // Registrar o serviço mockado
            _factory.ReplaceService(_mockProjectService.Object);

            // Preparar o conteúdo da requisição
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(projectDto),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/projects", jsonContent);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var returnedProject = await response.Content.ReadFromJsonAsync<ProjectDTO>();
            Assert.Equal(createdProject.Id, returnedProject.Id);
            Assert.Equal(createdProject.Name, returnedProject.Name);
            Assert.Equal(createdProject.Description, returnedProject.Description);
        }
    }
}
