using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Tests.Entities
{
    public class UserTests
    {
        [Fact]
        public void Constructor_WithValidData_ShouldCreateUserWithCorrectData()
        {
            // Arrange
            var name = "Test User";
            var email = "test@example.com";
            var isManager = true;

            // Act
            var user = new User(name, email, isManager);

            // Assert
            Assert.Equal(name, user.Name);
            Assert.Equal(email, user.Email);
            Assert.Equal(isManager, user.IsManager);
            Assert.NotEqual(Guid.Empty, user.Id);
            Assert.NotEqual(default, user.CreatedAt);
            Assert.Null(user.UpdatedAt);
            Assert.Empty(user.Projects);
        }

        [Fact]
        public void Constructor_WithDefaultIsManager_ShouldCreateNonManagerUser()
        {
            // Arrange
            var name = "Test User";
            var email = "test@example.com";

            // Act
            var user = new User(name, email);

            // Assert
            Assert.False(user.IsManager);
        }

        [Fact]
        public void Update_ShouldUpdateNameAndEmail()
        {
            // Arrange
            var user = new User("Original Name", "original@example.com");
            var newName = "Updated Name";
            var newEmail = "updated@example.com";

            // Act
            user.Update(newName, newEmail);

            // Assert
            Assert.Equal(newName, user.Name);
            Assert.Equal(newEmail, user.Email);
            Assert.NotNull(user.UpdatedAt);
        }

        [Fact]
        public void User_ProjectsCollection_ShouldBeInitialized()
        {
            // Arrange & Act
            var user = new User("Test User", "test@example.com");

            // Assert
            Assert.NotNull(user.Projects);
            Assert.Empty(user.Projects);
        }
    }
}
