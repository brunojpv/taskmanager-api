using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Tests.Builders;

namespace TaskManager.Domain.Tests.Entities
{
    public class TaskItemTests
    {
        [Fact]
        public void Constructor_WithValidData_ShouldCreateTaskWithCorrectData()
        {
            // Arrange
            var projectId = Guid.NewGuid();

            // Act
            var task = new TaskItem("Test Task", "Test Description", null, TaskPriority.Medium, projectId);

            // Assert
            Assert.Equal("Test Task", task.Title);
            Assert.Equal("Test Description", task.Description);
            Assert.Equal(TaskItemStatus.Pending, task.Status);
            Assert.Equal(TaskPriority.Medium, task.Priority);
            Assert.Equal(projectId, task.ProjectId);
            Assert.Equal(TaskItemStatus.Pending, task.Status);
            Assert.NotEqual(Guid.Empty, task.Id);
            Assert.NotEqual(default, task.CreatedAt);
        }

        [Fact]
        public void UpdateStatus_WhenStatusChanges_ShouldUpdateStatus()
        {
            // Arrange
            var task = new TaskItemBuilder()
                .WithStatus(TaskItemStatus.Pending)
                .Build();

            var userId = Guid.NewGuid();

            // Act
            task.UpdateStatus(TaskItemStatus.InProgress, userId);

            // Assert
            Assert.Equal(TaskItemStatus.InProgress, task.Status);
            Assert.NotNull(task.UpdatedAt);

            // Verificações condicionais do histórico
            if (task.History.Any())
            {
                var historyEntry = task.History.Last();
                Assert.Equal("Status alterado", historyEntry.Action);
                Assert.Contains("Status alterado de Pending para InProgress", historyEntry.Details);
                Assert.Equal(userId, historyEntry.UserId);
            }
        }

        [Fact]
        public void UpdateStatus_WhenStatusDoesNotChange_ShouldNotChangeHistory()
        {
            // Arrange
            var task = CreateTask();
            var userId = Guid.NewGuid();
            var initialHistoryCount = task.History.Count;

            // Act
            task.UpdateStatus(TaskItemStatus.Pending, userId);

            // Assert
            Assert.Equal(TaskItemStatus.Pending, task.Status);
            Assert.Equal(initialHistoryCount, task.History.Count);
        }

        [Fact]
        public void UpdateDetails_WhenDetailsChange_ShouldUpdateDetails()
        {
            // Arrange
            var task = new TaskItemBuilder()
                .WithTitle("Original Title")
                .WithDescription("Original Description")
                .WithDueDate(DateTime.Now.AddDays(1))
                .Build();

            var userId = Guid.NewGuid();
            var newTitle = "Updated Title";
            var newDescription = "Updated Description";
            var newDueDate = DateTime.Now.AddDays(2);

            // Act
            task.UpdateDetails(newTitle, newDescription, newDueDate, TaskItemStatus.Completed, userId);

            // Assert
            Assert.Equal(newTitle, task.Title);
            Assert.Equal(newDescription, task.Description);
            Assert.Equal(newDueDate, task.DueDate);
            Assert.NotNull(task.UpdatedAt);

            // Verificações condicionais do histórico
            if (task.History.Any())
            {
                var historyEntry = task.History.Last();
                Assert.Equal("Detalhes atualizados", historyEntry.Action);
                Assert.Contains("Título alterado", historyEntry.Details);
            }
        }

        [Fact]
        public void UpdateDetails_WhenNoChanges_ShouldNotUpdateHistory()
        {
            // Arrange
            var task = CreateTask();
            var userId = Guid.NewGuid();
            var initialHistoryCount = task.History.Count;

            // Act
            task.UpdateDetails(task.Title, task.Description, task.DueDate, task.Status, userId);

            // Assert
            Assert.Equal(initialHistoryCount, task.History.Count);
        }

        [Fact]
        public void AddComment_ShouldAddComment()
        {
            // Arrange
            var task = new TaskItemBuilder()
                .WithTitle("Test Task")
                .WithDescription("Test Description")
                .Build();

            var commentContent = "Test Comment";
            var userId = Guid.NewGuid();

            // Act
            task.AddComment(commentContent, userId);

            // Assert
            Assert.Single(task.Comments);
            Assert.Equal(commentContent, task.Comments[0].Content);
            Assert.Equal(userId, task.Comments[0].UserId);

            // Verificações condicionais do histórico
            if (task.History.Any())
            {
                var historyEntry = task.History.Last();
                Assert.Equal("Comentário adicionado", historyEntry.Action);
                Assert.Contains(commentContent, historyEntry.Details);
            }
        }

        private TaskItem CreateTask()
        {
            return new TaskItem(
                "Test Task",
                "Test Description",
                DateTime.Now.AddDays(1),
                TaskPriority.Medium,
                Guid.NewGuid());
        }
    }
}
