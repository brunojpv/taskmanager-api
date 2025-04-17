using Moq;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

using DomainTaskStatus = TaskManager.Domain.Enums.ActivityStatus;

namespace TaskManager.UnitTests
{
    public class TaskServiceTests
    {
        private readonly ActivityService _service;
        private readonly Mock<IActivityRepository> _repositoryMock;

        public TaskServiceTests()
        {
            _repositoryMock = new Mock<IActivityRepository>();
            _service = new ActivityService(_repositoryMock.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task AddAsync_Should_Call_Repository()
        {
            var task = new TaskItem
            {
                Title = "Nova tarefa",
                Description = "Descrição da tarefa",
                DueDate = DateTime.UtcNow.AddDays(1),
                Status = DomainTaskStatus.Pending,
                ProjectId = Guid.NewGuid()
            };

            await _service.AddAsync(task);

            _repositoryMock.Verify(r => r.AddAsync(task), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllAsync_Should_Return_Tasks()
        {
            var userId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetAllAsync(userId)).ReturnsAsync(new List<Task>());

            var result = await _service.GetAllAsync(userId);

            Assert.NotNull(result);
            _repositoryMock.Verify(r => r.GetAllAsync(userId), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetByIdAsync_Should_Return_Task()
        {
            var taskId = Guid.NewGuid();
            var task = new TaskItem
            {
                Id = taskId,
                Title = "Sample",
                Description = "Descrição para teste",
                DueDate = DateTime.UtcNow.AddDays(3),
                Status = DomainTaskStatus.InProgress,
                ProjectId = Guid.NewGuid()
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);

            var result = await _service.GetByIdAsync(taskId);

            Assert.NotNull(result);
            Assert.Equal("Sample", result?.Title);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteAsync_Should_Call_Repository()
        {
            var taskId = Guid.NewGuid();

            await _service.DeleteAsync(taskId);

            _repositoryMock.Verify(r => r.DeleteAsync(taskId), Times.Once);
        }
    }
}