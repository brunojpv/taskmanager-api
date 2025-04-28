using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Tests.Entities
{
    public class TaskItemTests
    {
        [Fact]
        public void Constructor_WithValidData_ShouldCreateTaskWithCorrectData()
        {
            // Arrange
            var title = "Test Task";
            var description = "Test Description";
            var dueDate = DateTime.Now.AddDays(1);
            var priority = TaskPriority.High;
            var projectId = Guid.NewGuid();

            // Act
            var task = new TaskItem(title, description, dueDate, priority, projectId);

            // Assert
            Assert.Equal(title, task.Title);
            Assert.Equal(description, task.Description);
            Assert.Equal(dueDate, task.DueDate);
            Assert.Equal(priority, task.Priority);
            Assert.Equal(projectId, task.ProjectId);
            Assert.Equal(TaskItemStatus.Pending, task.Status);
            Assert.NotEqual(Guid.Empty, task.Id);
            Assert.NotEqual(default, task.CreatedAt);
            Assert.Single(task.History); // Should have creation entry
        }

        [Fact]
        public void UpdateStatus_WhenStatusChanges_ShouldUpdateStatusAndAddHistoryEntry()
        {
            // Arrange
            var task = CreateTask();
            var userId = Guid.NewGuid();
            var initialHistoryCount = task.History.Count;
            var newStatus = TaskItemStatus.InProgress;

            // Act
            task.UpdateStatus(newStatus, userId);

            // Assert
            Assert.Equal(newStatus, task.Status);
            Assert.Equal(initialHistoryCount + 1, task.History.Count);
            Assert.NotNull(task.UpdatedAt);

            var historyEntry = task.History.Last();
            Assert.Equal("Status alterado", historyEntry.Action);
            Assert.Contains("Status alterado de Pending para InProgress", historyEntry.Details);
            Assert.Equal(userId, historyEntry.UserId);
        }

        [Fact]
        public void UpdateStatus_WhenStatusDoesNotChange_ShouldNotAddHistoryEntry()
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
        public void UpdateDetails_WhenDetailsChange_ShouldUpdateDetailsAndAddHistoryEntry()
        {
            // Arrange
            var task = CreateTask();
            var userId = Guid.NewGuid();
            var initialHistoryCount = task.History.Count;
            var newTitle = "Updated Title";
            var newDescription = "Updated Description";
            var newDueDate = DateTime.Now.AddDays(2);

            // Act
            task.UpdateDetails(newTitle, newDescription, newDueDate, TaskItemStatus.Completed, userId);

            // Assert
            Assert.Equal(newTitle, task.Title);
            Assert.Equal(newDescription, task.Description);
            Assert.Equal(newDueDate, task.DueDate);
            Assert.Equal(initialHistoryCount + 1, task.History.Count);
            Assert.NotNull(task.UpdatedAt);

            var historyEntry = task.History.Last();
            Assert.Equal("Detalhes atualizados", historyEntry.Action);
            Assert.Contains("Título alterado", historyEntry.Details);
        }

        [Fact]
        public void UpdateDetails_WhenNoChanges_ShouldNotAddHistoryEntry()
        {
            // Arrange
            var task = CreateTask();
            var userId = Guid.NewGuid();
            var initialHistoryCount = task.History.Count;

            // Act
            task.UpdateDetails(task.Title, task.Description, task.DueDate, TaskItemStatus.Completed, userId);

            // Assert
            Assert.Equal(initialHistoryCount, task.History.Count);
        }

        [Fact]
        public void AddComment_ShouldAddCommentAndHistoryEntry()
        {
            // Arrange
            var task = CreateTask();
            var userId = Guid.NewGuid();
            var initialHistoryCount = task.History.Count;
            var commentContent = "Test comment";

            // Act
            task.AddComment(commentContent, userId);

            // Assert
            Assert.Single(task.Comments);
            Assert.Equal(commentContent, task.Comments.First().Content);
            Assert.Equal(userId, task.Comments.First().UserId);
            Assert.Equal(initialHistoryCount + 1, task.History.Count);

            var historyEntry = task.History.Last();
            Assert.Equal("Comentário adicionado", historyEntry.Action);
            Assert.Contains(commentContent, historyEntry.Details);
        }

        // Helper methods
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
