using Moq;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Repositories;

namespace TaskManager.Application.Tests.Services
{
    public class ReportServiceTests
    {
        private readonly Mock<IReportRepository> _mockReportRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly ReportService _reportService;

        public ReportServiceTests()
        {
            _mockReportRepository = new Mock<IReportRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _reportService = new ReportService(_mockReportRepository.Object, _mockUserRepository.Object);
        }

        [Fact]
        public async Task GetUserTaskReportAsync_WithValidUser_ShouldReturnReport()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User("Test User", "test@example.com");
            var days = 30;

            var reportEntity = new UserTaskReport(
                userId,
                "Test User",
                "test@example.com",
                false,
                10, // totalTasks
                5,  // completedTasks
                3,  // inProgressTasks
                2,  // pendingTasks
                2,  // highPriorityTasks
                5,  // mediumPriorityTasks
                3,  // lowPriorityTasks
                1,  // overdueTasks
                1,  // tasksDueToday
                2,  // tasksDueThisWeek
                3,  // tasksDueNextWeek
                50.0, // completionRate
                2.5,  // averageCompletionTimeInDays
                5,    // completedTasksLastDays
                days
            );

            _mockUserRepository.Setup(r => r.GetByIdAsync(userId))
                .ReturnsAsync(user);

            _mockReportRepository.Setup(r => r.GetUserTaskReportAsync(userId, days))
                .ReturnsAsync(reportEntity);

            // Act
            var result = await _reportService.GetUserTaskReportAsync(userId, days);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal("Test User", result.UserName);
            Assert.Equal(10, result.TotalTasks);
            Assert.Equal(5, result.CompletedTasks);
            Assert.Equal(50.0, result.CompletionRate);
            Assert.Equal(5, result.CompletedTasksLast30Days);
        }

        [Fact]
        public async Task GetUserTaskReportAsync_WithInvalidUser_ShouldThrowException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mockUserRepository.Setup(r => r.GetByIdAsync(userId))
                .ReturnsAsync((User)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DomainException>(
                () => _reportService.GetUserTaskReportAsync(userId));

            Assert.Equal("Usuário não encontrado.", exception.Message);
        }

        [Fact]
        public async Task GetAllUsersTaskReportAsync_ShouldReturnAllReports()
        {
            // Arrange
            var days = 30;
            var reportsEntities = new List<UserTaskReport>
            {
                new UserTaskReport(
                    Guid.NewGuid(),
                    "User 1",
                    "user1@example.com",
                    false,
                    10, 5, 3, 2, 2, 5, 3, 1, 1, 2, 3, 50.0, 2.5, 5, days
                ),
                new UserTaskReport(
                    Guid.NewGuid(),
                    "User 2",
                    "user2@example.com",
                    true,
                    15, 8, 4, 3, 4, 7, 4, 2, 1, 3, 4, 53.3, 3.2, 8, days
                )
            };

            _mockReportRepository.Setup(r => r.GetAllUsersTaskReportAsync(days))
                .ReturnsAsync(reportsEntities);

            // Act
            var results = await _reportService.GetAllUsersTaskReportAsync(days);

            // Assert
            Assert.NotNull(results);
            Assert.Equal(2, results.Count);
            Assert.Equal("User 1", results[0].UserName);
            Assert.Equal("User 2", results[1].UserName);
            Assert.Equal(5, results[0].CompletedTasks);
            Assert.Equal(8, results[1].CompletedTasks);
        }

        [Fact]
        public async Task GetTeamPerformanceReportAsync_ShouldReturnTeamReport()
        {
            // Arrange
            var days = 30;
            var totalCompletedTasks = 100;
            var totalUsers = 10;
            var averageTasksPerUser = 10.0;

            _mockReportRepository.Setup(r => r.GetTotalCompletedTasksAsync(days))
                .ReturnsAsync(totalCompletedTasks);
            _mockReportRepository.Setup(r => r.GetTotalUsersAsync())
                .ReturnsAsync(totalUsers);
            _mockReportRepository.Setup(r => r.GetAverageCompletedTasksPerUserAsync(days))
                .ReturnsAsync(averageTasksPerUser);

            // Act
            var result = await _reportService.GetTeamPerformanceReportAsync(days);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(totalCompletedTasks, result.TotalCompletedTasks);
            Assert.Equal(totalUsers, result.TotalUsers);
            Assert.Equal(averageTasksPerUser, result.AverageCompletedTasksPerUser);
            Assert.Equal(days, result.DaysInReport);
        }
    }
}
