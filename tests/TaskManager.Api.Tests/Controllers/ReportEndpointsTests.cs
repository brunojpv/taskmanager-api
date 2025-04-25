using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;
using System.Net.Http.Json;
using TaskManager.Api.Tests.Helpers;
using TaskManager.Application.DTOs;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Repositories;

namespace TaskManager.Api.Tests.Controllers
{
    public class ReportEndpointsTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly TestWebApplicationFactory _factory;
        private readonly Mock<IReportService> _mockReportService;
        private readonly Mock<IUserRepository> _mockUserRepository;

        public ReportEndpointsTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _mockReportService = new Mock<IReportService>();
            _mockUserRepository = new Mock<IUserRepository>();
        }

        [Fact]
        public async Task GetTeamPerformanceReport_WhenUserIsManager_ReturnsOkResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var days = 30;

            // Criar um usuário gerente válido
            var manager = new User("Test Manager", "manager@example.com", true);

            var report = new PerformanceReportDTO
            {
                TotalCompletedTasks = 150,
                TotalUsers = 10,
                AverageCompletedTasksPerUser = 15.0,
                ReportDate = DateTime.UtcNow,
                DaysInReport = days
            };

            // Configurar o mock para retornar o gerente quando solicitado pelo ID específico
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(manager);

            // Configurar o mock do serviço de relatórios
            _mockReportService.Setup(service => service.GetTeamPerformanceReportAsync(days))
                .ReturnsAsync(report);

            // Registrar os serviços mockados
            _factory.ReplaceService(_mockUserRepository.Object);
            _factory.ReplaceService(_mockReportService.Object);

            // Configurar o header da requisição
            _client.DefaultRequestHeaders.Add("User-Id", userId.ToString());

            // Act
            var response = await _client.GetAsync($"/api/reports/performance/team?days={days}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var returnedReport = await response.Content.ReadFromJsonAsync<PerformanceReportDTO>();
            Assert.Equal(150, returnedReport.TotalCompletedTasks);
            Assert.Equal(10, returnedReport.TotalUsers);
            Assert.Equal(15.0, returnedReport.AverageCompletedTasksPerUser);
        }

        [Fact]
        public async Task GetUserTaskReport_WhenUserIsManagerAndExists_ReturnsOkResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var days = 30;

            // Criar um gerente válido
            var manager = new User("Test Manager", "manager@example.com", true);

            var report = new UserTaskReportDto
            {
                UserId = userId,
                UserName = "Test Manager",
                Email = "manager@example.com",
                IsManager = true,
                TotalTasks = 25,
                CompletedTasks = 15,
                InProgressTasks = 5,
                PendingTasks = 5,
                HighPriorityTasks = 8,
                MediumPriorityTasks = 10,
                LowPriorityTasks = 7,
                CompletionRate = 60.0,
                DaysInReport = days
            };

            // Configurar o mock do repositório
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(manager);

            // Configurar o mock do serviço
            _mockReportService.Setup(service => service.GetUserTaskReportAsync(userId, days))
                .ReturnsAsync(report);

            // Registrar os serviços mockados
            _factory.ReplaceService(_mockUserRepository.Object);
            _factory.ReplaceService(_mockReportService.Object);

            // Act
            var response = await _client.GetAsync($"/api/reports/user/{userId}?days={days}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var returnedReport = await response.Content.ReadFromJsonAsync<UserTaskReportDto>();
            Assert.Equal(userId, returnedReport.UserId);
            Assert.Equal("Test Manager", returnedReport.UserName);
            Assert.Equal(25, returnedReport.TotalTasks);
            Assert.Equal(15, returnedReport.CompletedTasks);
        }

        [Fact]
        public async Task GetAllUsersTaskReport_ReturnsOkResult_WithReports()
        {
            // Arrange
            var days = 30;
            var reports = new List<UserTaskReportDto>
            {
                new UserTaskReportDto
                {
                    UserId = Guid.NewGuid(),
                    UserName = "User 1",
                    TotalTasks = 20,
                    CompletedTasks = 10,
                    CompletionRate = 50.0
                },
                new UserTaskReportDto
                {
                    UserId = Guid.NewGuid(),
                    UserName = "User 2",
                    TotalTasks = 15,
                    CompletedTasks = 12,
                    CompletionRate = 80.0
                }
            };

            // Configurar o mock do serviço de relatórios
            _mockReportService.Setup(service => service.GetAllUsersTaskReportAsync(days))
                .ReturnsAsync(reports);

            // Configurar o usuário como gerente para ter acesso
            var managerId = Guid.NewGuid();
            var manager = new User("Manager", "manager@example.com", true);

            // Configurar o mock do repositório de usuários
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(managerId))
                .ReturnsAsync(manager);

            // Configurar o mock para verificação de perfil de gerente
            _mockUserRepository.Setup(repo => repo.IsManagerAsync(managerId))
                .ReturnsAsync(true);

            // Registrar os serviços mockados
            _factory.ReplaceService(_mockReportService.Object);
            _factory.ReplaceService(_mockUserRepository.Object);

            // Configurar o header da requisição
            _client.DefaultRequestHeaders.Add("User-Id", managerId.ToString());

            // Act
            var response = await _client.GetAsync($"/api/reports/users/all?days={days}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var returnedReports = await response.Content.ReadFromJsonAsync<List<UserTaskReportDto>>();
            Assert.Equal(2, returnedReports.Count);
            Assert.Equal("User 1", returnedReports[0].UserName);
            Assert.Equal("User 2", returnedReports[1].UserName);
        }
    }
}
