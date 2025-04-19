using TaskManager.Infrastructure.Data.Builders;

namespace TaskManager.UnitTests.Builders
{
    public class UserBuilderTests
    {
        [Fact]
        public void Should_Create_User_With_Valid_Data()
        {
            var user = new UserBuilder()
                .WithName("Test User")
                .WithEmail("test@example.com")
                .WithPassword("password")
                .Build();

            Assert.NotNull(user);
            Assert.Equal("Test User", user.Name);
            Assert.Equal("test@example.com", user.Email);
            Assert.NotEmpty(user.PasswordHash);
        }
    }
}
