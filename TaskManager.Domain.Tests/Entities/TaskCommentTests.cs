using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Tests.Entities
{
    public class TaskCommentTests
    {
        [Fact]
        public void Constructor_WithValidData_ShouldCreateCommentWithCorrectData()
        {
            // Arrange
            var content = "Test comment content";
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            // Act
            var comment = new TaskComment(content, taskId, userId);

            // Assert
            Assert.Equal(content, comment.Content);
            Assert.Equal(taskId, comment.TaskId);
            Assert.Equal(userId, comment.UserId);
            Assert.NotEqual(Guid.Empty, comment.Id);
            Assert.NotEqual(default, comment.CreatedAt);
            Assert.Null(comment.UpdatedAt);
        }

        [Fact]
        public void Update_ShouldUpdateContent()
        {
            // Arrange
            var comment = new TaskComment("Original content", Guid.NewGuid(), Guid.NewGuid());
            var newContent = "Updated content";

            // Act
            comment.Update(newContent);

            // Assert
            Assert.Equal(newContent, comment.Content);
            Assert.NotNull(comment.UpdatedAt);
        }
    }
}
