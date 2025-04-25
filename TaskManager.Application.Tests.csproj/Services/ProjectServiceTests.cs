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
    public class ProjectServiceTests
    {
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly ProjectService _projectService;

        public ProjectServiceTests()
        {
            _mockProjectRepository = new Mock<IProjectRepository>();
            _projectService = new ProjectService(_mockProjectRepository.Object);
        }

        [Fact]
        public async Task GetAllByUserIdAsync_ShouldReturnProjectDTOs()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var projects = new List<Project>
            {
                new ProjectBuilder()
                    .WithName("Project 1")
                    .WithDescription("Description 1")
                    .WithOwner(userId)
                    .Build(),
                new ProjectBuilder()
                    .WithName("Project 2")
                    .WithDescription("Description 2")
                    .WithOwner(userId)
                    .Build()
            };

            _mockProjectRepository.Setup(r => r.GetAllByUserIdAsync(userId))
                .ReturnsAsync(projects);
            _mockProjectRepository.Setup(r => r.CountTasksAsync(It.IsAny<Guid>()))
                .ReturnsAsync(5);

            // Act
            var result = await _projectService.GetAllByUserIdAsync(userId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(projects[0].Name, result[0].Name);
            Assert.Equal(projects[1].Name, result[1].Name);
            Assert.Equal(5, result[0].TaskCount);
            Assert.Equal(5, result[1].TaskCount);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateAndReturnProjectDTO()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var projectDto = new ProjectCreateDTO
            {
                Name = "New Project",
                Description = "New Description",
                UserId = userId
            };

            _mockProjectRepository.Setup(r => r.AddAsync(It.IsAny<Project>()))
                .ReturnsAsync((Project p) => p);

            // Act
            var result = await _projectService.CreateAsync(projectDto);

            // Assert
            Assert.Equal(projectDto.Name, result.Name);
            Assert.Equal(projectDto.Description, result.Description);
            Assert.Equal(0, result.TaskCount);
            _mockProjectRepository.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenProjectHasPendingTasks_ShouldThrowException()
        {
            // Arrange
            var projectId = Guid.NewGuid();

            // Criar um projeto com ID personalizado
            var project = new ProjectBuilder()
                .WithName("Test Project")
                .WithDescription("Test Description")
                .WithOwner(Guid.NewGuid())
                .WithId(projectId)
                .Build();

            // Adicionar uma tarefa pendente ao projeto
            var task = new TaskItemBuilder()
                .WithTitle("Test Task")
                .WithDescription("Test Description")
                .WithDueDate(DateTime.Now.AddDays(1))
                .WithPriority(TaskPriority.Medium)
                .InProject(projectId)
                .Build();

            project.AddTask(task);

            _mockProjectRepository.Setup(r => r.GetByIdWithTasksAsync(projectId))
                .ReturnsAsync(project);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DomainException>(
                () => _projectService.DeleteAsync(projectId));

            Assert.Equal("Não é possível remover um projeto com tarefas pendentes. Complete ou remova as tarefas primeiro.",
                exception.Message);

            _mockProjectRepository.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_WhenProjectHasNoTasks_ShouldDeleteProject()
        {
            // Arrange
            var projectId = Guid.NewGuid();

            // Criar um projeto com ID personalizado
            var project = new ProjectBuilder()
                .WithName("Test Project")
                .WithDescription("Test Description")
                .WithOwner(Guid.NewGuid())
                .WithId(projectId)
                .Build();

            _mockProjectRepository.Setup(r => r.GetByIdWithTasksAsync(projectId))
                .ReturnsAsync(project);
            _mockProjectRepository.Setup(r => r.DeleteAsync(projectId))
                .ReturnsAsync(true);

            // Act
            var result = await _projectService.DeleteAsync(projectId);

            // Assert
            Assert.True(result);
            _mockProjectRepository.Verify(r => r.DeleteAsync(projectId), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldUpdateProject()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var projectDto = new ProjectUpdateDTO
            {
                Id = projectId,
                Name = "Updated Project",
                Description = "Updated Description"
            };

            var originalProject = new ProjectBuilder()
                .WithName("Original Project")
                .WithDescription("Original Description")
                .WithOwner(Guid.NewGuid())
                .WithId(projectId)
                .Build();

            _mockProjectRepository.Setup(r => r.GetByIdWithTasksAsync(projectId))
                .ReturnsAsync(originalProject);

            // Act
            var result = await _projectService.UpdateAsync(projectDto);

            // Assert
            Assert.Equal(projectDto.Name, result.Name);
            Assert.Equal(projectDto.Description, result.Description);

            // Verify project object was updated
            Assert.Equal(projectDto.Name, originalProject.Name);
            Assert.Equal(projectDto.Description, originalProject.Description);

            // Verify the repository method was called
            _mockProjectRepository.Verify(r => r.UpdateAsync(originalProject), Times.Once);
        }
    }
}
