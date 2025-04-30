using AutoMapper;
using Moq;
using TaskManager.Application.DTOs;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Tests.Builders;

namespace TaskManager.Application.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _mockTaskRepository;
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<ITaskHistoryRepository> _mockHistoryRepository;
        private readonly Mock<ITaskCommentRepository> _mockCommentRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _mockTaskRepository = new Mock<ITaskRepository>();
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockHistoryRepository = new Mock<ITaskHistoryRepository>();
            _mockCommentRepository = new Mock<ITaskCommentRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();

            _taskService = new TaskService(
                _mockTaskRepository.Object,
                _mockProjectRepository.Object,
                _mockHistoryRepository.Object,
                _mockCommentRepository.Object,
                _mockUserRepository.Object,
                _mockMapper.Object);
        }

        [Fact]
        public async Task CreateAsync_WhenProjectHasLessThan20Tasks_ShouldCreateTask()
        {
            // Arrange
            var projectId = Guid.NewGuid();

            var project = new ProjectBuilder()
                .WithName("Test Project")
                .WithDescription("Test Description")
                .WithOwner(Guid.NewGuid())
                .WithId(projectId)
                .Build();

            var taskDto = new TaskCreateDTO
            {
                Title = "New Task",
                Description = "New Description",
                DueDate = DateTime.Now.AddDays(1),
                Priority = TaskPriority.High,
                ProjectId = projectId
            };

            _mockProjectRepository.Setup(r => r.GetByIdWithTasksAsync(projectId))
                .ReturnsAsync(project);

            TaskItem savedTask = null;
            _mockTaskRepository.Setup(r => r.AddAsync(It.IsAny<TaskItem>()))
                .Callback<TaskItem>(t => savedTask = t)
                .ReturnsAsync((TaskItem t) => t);

            _mockMapper.Setup(m => m.Map<TaskDTO>(It.IsAny<TaskItem>()))
                .Returns((TaskItem source) => new TaskDTO
                {
                    Id = source.Id,
                    Title = source.Title,
                    Description = source.Description,
                    DueDate = source.DueDate,
                    Status = source.Status,
                    Priority = source.Priority,
                    ProjectId = source.ProjectId,
                    CreatedAt = source.CreatedAt,
                    UpdatedAt = source.UpdatedAt,
                    Comments = new List<TaskCommentDTO>()
                });

            // Act
            var result = await _taskService.CreateAsync(taskDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskDto.Title, result.Title);
            Assert.Equal(taskDto.Description, result.Description);
            Assert.Equal(taskDto.DueDate?.Date, result.DueDate?.Date);
            Assert.Equal(taskDto.Priority, result.Priority);
            Assert.Equal(TaskItemStatus.Pending, result.Status);
            Assert.Equal(taskDto.ProjectId, result.ProjectId);
            Assert.Single(project.Tasks);

            _mockTaskRepository.Verify(r => r.AddAsync(It.Is<TaskItem>(
                t => t.Title == taskDto.Title &&
                     t.Description == taskDto.Description &&
                     t.DueDate == taskDto.DueDate &&
                     t.Priority == taskDto.Priority &&
                     t.ProjectId == taskDto.ProjectId
            )), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenProjectHas20Tasks_ShouldThrowException()
        {
            var projectId = Guid.NewGuid();

            var project = new ProjectBuilder()
                .WithName("Test Project")
                .WithDescription("Test Description")
                .WithOwner(Guid.NewGuid())
                .WithId(projectId)
                .Build();

            for (int i = 0; i < 20; i++)
            {
                var task = new TaskItemBuilder()
                    .WithTitle($"Task {i}")
                    .WithDescription("Description")
                    .WithDueDate(DateTime.Now.AddDays(1))
                    .WithPriority(TaskPriority.Medium)
                    .InProject(projectId)
                    .Build();

                project.AddTask(task);
            }

            var taskDto = new TaskCreateDTO
            {
                Title = "New Task",
                Description = "New Description",
                DueDate = DateTime.Now.AddDays(1),
                Priority = TaskPriority.High,
                ProjectId = projectId
            };

            _mockProjectRepository.Setup(r => r.GetByIdWithTasksAsync(projectId))
                .ReturnsAsync(project);

            var exception = await Assert.ThrowsAsync<DomainException>(
                () => _taskService.CreateAsync(taskDto));

            Assert.Equal("Limite máximo de 20 tarefas por projeto atingido.",
                exception.Message);

            _mockTaskRepository.Verify(r => r.AddAsync(It.IsAny<TaskItem>()), Times.Never);

            _mockMapper.Verify(m => m.Map<TaskDTO>(It.IsAny<TaskItem>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldUpdateTask()
        {
            var taskId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var task = new TaskItemBuilder()
                .WithTitle("Original Task")
                .WithDescription("Original Description")
                .WithDueDate(DateTime.Now.AddDays(1))
                .WithPriority(TaskPriority.Medium)
                .InProject(projectId)
                .WithId(taskId)
                .Build();

            var taskDto = new TaskUpdateDTO
            {
                Id = taskId,
                Title = "Updated Task",
                Description = "Updated Description",
                DueDate = DateTime.Now.AddDays(2),
                Status = TaskItemStatus.InProgress,
                UserId = userId
            };

            var expectedTaskDto = new TaskDTO
            {
                Id = taskId,
                Title = "Updated Task",
                Description = "Updated Description",
                DueDate = DateTime.Now.AddDays(2),
                Status = TaskItemStatus.InProgress,
                Priority = TaskPriority.Medium,
                ProjectId = projectId,
                Comments = new List<TaskCommentDTO>()
            };

            _mockTaskRepository.Setup(r => r.GetByIdAsync(taskId))
                .ReturnsAsync(task);
            _mockTaskRepository.Setup(r => r.UpdateAsync(It.IsAny<TaskItem>()))
                .Returns(Task.CompletedTask);
            _mockCommentRepository.Setup(r => r.GetAllByTaskIdAsync(taskId))
                .ReturnsAsync(new List<TaskComment>());

            _mockMapper.Setup(m => m.Map<TaskDTO>(It.IsAny<TaskItem>()))
                .Returns(expectedTaskDto);
            _mockMapper.Setup(m => m.Map<List<TaskCommentDTO>>(It.IsAny<List<TaskComment>>()))
                .Returns(new List<TaskCommentDTO>());

            var result = await _taskService.UpdateAsync(taskDto);

            Assert.NotNull(result);
            Assert.Equal(taskDto.Title, result.Title);
            Assert.Equal(taskDto.Description, result.Description);
            Assert.Equal(taskDto.DueDate?.Date, result.DueDate?.Date);
            Assert.Equal(taskDto.Status, result.Status);
            Assert.Equal(taskDto.Title, task.Title);
            Assert.Equal(taskDto.Description, task.Description);
            Assert.Equal(taskDto.DueDate, task.DueDate);
            Assert.Equal(taskDto.Status, task.Status);

            _mockTaskRepository.Verify(r => r.UpdateAsync(task), Times.Once);

            _mockMapper.Verify(m => m.Map<TaskDTO>(It.IsAny<TaskItem>()), Times.Once);
            _mockMapper.Verify(m => m.Map<List<TaskCommentDTO>>(It.IsAny<List<TaskComment>>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldDeleteTask()
        {
            var taskId = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            var task = new TaskItemBuilder()
                .WithTitle("Test Task")
                .WithDescription("Test Description")
                .WithDueDate(DateTime.Now.AddDays(1))
                .WithPriority(TaskPriority.Medium)
                .InProject(projectId)
                .WithId(taskId)
                .Build();

            var project = new ProjectBuilder()
                .WithName("Test Project")
                .WithDescription("Test Description")
                .WithOwner(Guid.NewGuid())
                .WithId(projectId)
                .WithTask(task)
                .Build();

            _mockTaskRepository.Setup(r => r.GetByIdAsync(taskId))
                .ReturnsAsync(task);
            _mockProjectRepository.Setup(r => r.GetByIdWithTasksAsync(projectId))
                .ReturnsAsync(project);
            _mockProjectRepository.Setup(r => r.UpdateAsync(It.IsAny<Project>()))
                .Returns(Task.CompletedTask);
            _mockTaskRepository.Setup(r => r.DeleteAsync(task))
                .ReturnsAsync(true);

            var result = await _taskService.DeleteAsync(taskId);

            Assert.True(result);

            _mockTaskRepository.Verify(r => r.DeleteAsync(task), Times.Once);
        }
    }
}
