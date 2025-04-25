using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TaskManager.Api.Tests.Helpers;
using TaskManager.Application.DTOs;
using TaskManager.Application.Services;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Api.Tests.Controllers
{
    public class TaskEndpointsTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly TestWebApplicationFactory _factory;
        private readonly Mock<ITaskService> _mockTaskService;

        public TaskEndpointsTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _mockTaskService = new Mock<ITaskService>();
        }

        [Fact]
        public async Task GetTasksByProjectId_ReturnsOkResult_WithTasks()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var tasks = new List<TaskDTO>
            {
                new TaskDTO
                {
                    Id = Guid.NewGuid(),
                    Title = "Task 1",
                    Description = "Description 1",
                    CreatedAt = DateTime.UtcNow,
                    DueDate = DateTime.UtcNow.AddDays(5),
                    Status = TaskItemStatus.Pending,
                    Priority = TaskPriority.Medium,
                    ProjectId = projectId,
                    Comments = new List<TaskCommentDTO>()
                },
                new TaskDTO
                {
                    Id = Guid.NewGuid(),
                    Title = "Task 2",
                    Description = "Description 2",
                    CreatedAt = DateTime.UtcNow,
                    DueDate = DateTime.UtcNow.AddDays(10),
                    Status = TaskItemStatus.InProgress,
                    Priority = TaskPriority.High,
                    ProjectId = projectId,
                    Comments = new List<TaskCommentDTO>()
                }
            };

            // Configurar o mock do serviço de tarefas
            _mockTaskService.Setup(service => service.GetAllByProjectIdAsync(projectId))
                .ReturnsAsync(tasks);

            // Registrar o serviço mockado
            _factory.ReplaceService(_mockTaskService.Object);

            // Act
            var response = await _client.GetAsync($"/api/tasks/project/{projectId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var returnedTasks = await response.Content.ReadFromJsonAsync<List<TaskDTO>>();
            Assert.Equal(2, returnedTasks.Count);
            Assert.Equal("Task 1", returnedTasks[0].Title);
            Assert.Equal("Task 2", returnedTasks[1].Title);
        }

        [Fact]
        public async Task GetTasksByProjectId_ThrowsDomainException_ReturnsBadRequest()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var errorMessage = "Project not found";

            // Configurar o mock para lançar uma exceção
            _mockTaskService.Setup(service => service.GetAllByProjectIdAsync(projectId))
                .ThrowsAsync(new DomainException(errorMessage));

            // Registrar o serviço mockado
            _factory.ReplaceService(_mockTaskService.Object);

            // Act
            var response = await _client.GetAsync($"/api/tasks/project/{projectId}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains(errorMessage, content);
        }

        [Fact]
        public async Task GetTaskById_ReturnsOkResult_WithTask()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var task = new TaskDTO
            {
                Id = taskId,
                Title = "Task 1",
                Description = "Description 1",
                CreatedAt = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(5),
                Status = TaskItemStatus.Pending,
                Priority = TaskPriority.Medium,
                ProjectId = projectId,
                Comments = new List<TaskCommentDTO>()
            };

            // Configurar o mock para retornar a tarefa específica
            _mockTaskService.Setup(service => service.GetByIdAsync(taskId))
                .ReturnsAsync(task);

            // Registrar o serviço mockado
            _factory.ReplaceService(_mockTaskService.Object);

            // Act
            var response = await _client.GetAsync($"/api/tasks/{taskId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var returnedTask = await response.Content.ReadFromJsonAsync<TaskDTO>();
            Assert.Equal(taskId, returnedTask.Id);
            Assert.Equal("Task 1", returnedTask.Title);
            Assert.Equal(projectId, returnedTask.ProjectId);
        }

        [Fact]
        public async Task CreateTask_WithValidData_ReturnsCreatedResult()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var taskDto = new TaskCreateDTO
            {
                Title = "New Task",
                Description = "Task Description",
                DueDate = DateTime.UtcNow.AddDays(5),
                Priority = TaskPriority.High,
                ProjectId = projectId
            };

            var createdTask = new TaskDTO
            {
                Id = Guid.NewGuid(),
                Title = taskDto.Title,
                Description = taskDto.Description,
                CreatedAt = DateTime.UtcNow,
                DueDate = taskDto.DueDate,
                Status = TaskItemStatus.Pending,
                Priority = taskDto.Priority,
                ProjectId = projectId,
                Comments = new List<TaskCommentDTO>()
            };

            // Configurar o mock para retornar a tarefa criada
            _mockTaskService.Setup(service => service.CreateAsync(It.IsAny<TaskCreateDTO>()))
                .ReturnsAsync(createdTask);

            // Registrar o serviço mockado
            _factory.ReplaceService(_mockTaskService.Object);

            // Preparar o conteúdo da requisição
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(taskDto),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/tasks", jsonContent);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var returnedTask = await response.Content.ReadFromJsonAsync<TaskDTO>();
            Assert.Equal(createdTask.Id, returnedTask.Id);
            Assert.Equal(createdTask.Title, returnedTask.Title);
            Assert.Equal(createdTask.Description, returnedTask.Description);
        }
    }
}
