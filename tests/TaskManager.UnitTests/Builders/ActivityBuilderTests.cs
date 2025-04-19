using TaskManager.Domain.Enums;
using TaskManager.Infrastructure.Data.Builders;

namespace TaskManager.UnitTests.Builders
{
    public class ActivityBuilderTests
    {
        [Fact]
        public void Should_Create_Activity_With_Valid_Fields()
        {
            var user = new UserBuilder()
                .WithName("Dev")
                .WithEmail("dev@example.com")
                .WithPassword("secure")
                .Build();

            var project = new ProjectBuilder()
                .WithName("Project X")
                .WithDescription("Builder test")
                .WithUser(user)
                .Build();

            var activity = new ActivityBuilder()
                .WithTitle("Sample Task")
                .WithDescription("Description")
                .WithDueDate(DateTime.UtcNow.AddDays(1))
                .WithPriority(ActivityPriority.High)
                .WithStatus(ActivityStatus.Pending)
                .WithProject(project)
                .Build();

            Assert.NotNull(activity);
            Assert.Equal("Sample Task", activity.Title);
            Assert.Equal(ActivityPriority.High, activity.Priority);
            Assert.Equal(ActivityStatus.Pending, activity.Status);
            Assert.Equal(project, activity.Project);
        }
    }
}
