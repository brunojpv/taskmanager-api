using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Domain.Tests.Entities
{
    public class ProjectTests
    {
        [Fact]
        public void Constructor_WithValidData_ShouldCreateProjectWithCorrectData()
        {
            // Arrange
            var name = "Test Project";
            var description = "Test Description";
            var userId = Guid.NewGuid();

            // Act
            var project = new Project(name, description, userId);

            // Assert
            Assert.Equal(name, project.Name);
            Assert.Equal(description, project.Description);
            Assert.Equal(userId, project.UserId);
            Assert.NotEqual(Guid.Empty, project.Id);
            Assert.NotEqual(default, project.CreatedAt);
            Assert.Empty(project.Tasks);
        }

        [Fact]
        public void AddTask_WhenProjectHasLessThan20Tasks_ShouldAddTask()
        {
            // Arrange
            var project = CreateProject();
            var task = CreateTask(project.Id);

            // Act
            project.AddTask(task);

            // Assert
            Assert.Single(project.Tasks);
            Assert.Equal(task, project.Tasks.First());
        }

        [Fact]
        public void AddTask_WhenProjectHas20Tasks_ShouldThrowException()
        {
            // Arrange
            var project = CreateProject();

            for (int i = 0; i < 20; i++)
            {
                project.AddTask(CreateTask(project.Id));
            }

            var extraTask = CreateTask(project.Id);

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => project.AddTask(extraTask));
            Assert.Equal("Limite máximo de 20 tarefas por projeto atingido.", exception.Message);
        }

        [Fact]
        public void RemoveTask_WhenTaskExists_ShouldRemoveTask()
        {
            // Arrange
            var project = CreateProject();
            var task = CreateTask(project.Id);
            project.AddTask(task);

            // Act
            project.RemoveTask(task.Id);

            // Assert
            Assert.Empty(project.Tasks);
        }

        [Fact]
        public void RemoveTask_WhenTaskDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var project = CreateProject();
            var nonExistentTaskId = Guid.NewGuid();

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => project.RemoveTask(nonExistentTaskId));
            Assert.Equal("Tarefa não encontrada.", exception.Message);
        }

        [Fact]
        public void HasPendingTasks_WhenHasPendingTasks_ShouldReturnTrue()
        {
            // Arrange
            var project = CreateProject();
            var task = CreateTask(project.Id);
            project.AddTask(task);

            // Act
            var result = project.HasPendingTasks();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasPendingTasks_WhenNoTasksOrAllCompleted_ShouldReturnFalse()
        {
            // Arrange
            var project = CreateProject();
            var userId = Guid.NewGuid();

            var task = CreateTask(project.Id);
            task.UpdateStatus(TaskItemStatus.Completed, userId);
            project.AddTask(task);

            // Act
            var result = project.HasPendingTasks();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Update_ShouldUpdateNameAndDescription()
        {
            // Arrange
            var project = CreateProject();
            var newName = "Updated Name";
            var newDescription = "Updated Description";

            // Act
            project.Update(newName, newDescription);

            // Assert
            Assert.Equal(newName, project.Name);
            Assert.Equal(newDescription, project.Description);
            Assert.NotNull(project.UpdatedAt);
        }

        private Project CreateProject()
        {
            return new Project("Test Project", "Test Description", Guid.NewGuid());
        }

        private TaskItem CreateTask(Guid projectId)
        {
            return new TaskItem(
                "Test Task",
                "Test Description",
                DateTime.Now.AddDays(1),
                TaskPriority.Medium,
                projectId);
        }
    }
}
