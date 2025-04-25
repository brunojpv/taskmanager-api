using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Tests.Entities
{
    public class TaskHistoryEntryTests
    {
        [Fact]
        public void Constructor_WithFullData_ShouldCreateHistoryEntryWithCorrectData()
        {
            // Arrange
            var action = "Status changed";
            var details = "Status changed from Pending to InProgress";
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            // Act
            var historyEntry = new TaskHistoryEntry(action, details, taskId, userId);

            // Assert
            Assert.Equal(action, historyEntry.Action);
            Assert.Equal(details, historyEntry.Details);
            Assert.Equal(taskId, historyEntry.TaskId);
            Assert.Equal(userId, historyEntry.UserId);
            Assert.NotEqual(Guid.Empty, historyEntry.Id);
            Assert.NotEqual(default, historyEntry.CreatedAt);
            Assert.NotEqual(default, historyEntry.Timestamp);
            Assert.Null(historyEntry.UpdatedAt);
        }

        [Fact]
        public void Constructor_WithNullUserId_ShouldCreateSystemHistoryEntry()
        {
            // Arrange
            var action = "Task created";
            string details = null;
            var taskId = Guid.NewGuid();
            Guid? userId = null;

            // Act
            var historyEntry = new TaskHistoryEntry(action, details, taskId, userId);

            // Assert
            Assert.Equal(action, historyEntry.Action);
            Assert.Null(historyEntry.Details);
            Assert.Equal(taskId, historyEntry.TaskId);
            Assert.Null(historyEntry.UserId);
            Assert.NotEqual(default, historyEntry.Timestamp);
        }

        [Fact]
        public void Constructor_ShouldSetTimestampToCurrentTime()
        {
            // Arrange
            var beforeCreation = DateTime.UtcNow.AddSeconds(-1);

            // Act
            var historyEntry = new TaskHistoryEntry("Test action", null, Guid.NewGuid(), null);

            var afterCreation = DateTime.UtcNow.AddSeconds(1);

            // Assert
            Assert.True(historyEntry.Timestamp >= beforeCreation);
            Assert.True(historyEntry.Timestamp <= afterCreation);
        }
    }
}
