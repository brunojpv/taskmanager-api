using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Enums;
using TaskManager.Infrastructure.Data;
using TaskManager.Infrastructure.Data.Builders;

namespace TaskManager.IntegrationTests
{
    public class ActivityIntegrationTests
    {
        private static DbContextOptions<AppDbContext> CreateInMemoryOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
                .Options;
        }

        [Fact]
        public async Task Should_Create_And_Retrieve_Activity()
        {
            var options = CreateInMemoryOptions();

            using (var context = new AppDbContext(options))
            {
                // Arrange
                var user = new UserBuilder()
                    .WithName("Bruno Test")
                    .WithEmail("bruno@test.com")
                    .WithPassword("123456")
                    .Build();

                var project = new ProjectBuilder()
                    .WithName("Projeto A")
                    .WithUser(user)
                    .Build();

                var activity = new ActivityBuilder()
                    .WithTitle("Nova tarefa")
                    .WithDescription("Atividade de teste")
                    .WithPriority(ActivityPriority.High)
                    .WithStatus(ActivityStatus.Pending)
                    .WithDueDate(DateTime.UtcNow.AddDays(3))
                    .WithProject(project)
                    .Build();

                context.Users.Add(user);
                context.Projects.Add(project);
                context.Activities.Add(activity);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                // Act
                var retrieved = await context.Activities
                    .Include(a => a.Project)
                    .ThenInclude(p => p.User!)
                    .FirstOrDefaultAsync();

                // Assert
                Assert.NotNull(retrieved);
                Assert.NotNull(retrieved!.Project);
                Assert.NotNull(retrieved.Project!.User);

                Assert.Equal("Nova tarefa", retrieved.Title);
                Assert.Equal(ActivityPriority.High, retrieved.Priority);
                Assert.Equal("Bruno Test", retrieved.Project.User.Name);
            }
        }
    }
}
