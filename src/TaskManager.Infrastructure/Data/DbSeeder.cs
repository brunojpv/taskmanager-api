using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Infrastructure.Data.Builders;

namespace TaskManager.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                var user = new UserBuilder()
                    .WithName("Bruno Dev")
                    .WithEmail("bruno@dev.com")
                    .WithPassword("123456")
                    .Build();

                var project = new ProjectBuilder()
                    .WithName("Projeto de Teste")
                    .WithDescription("Criado automaticamente via seed")
                    .WithUser(user)
                    .Build();

                var activities = new List<Activity>
                {
                    new ActivityBuilder()
                        .WithTitle("Tarefa pendente")
                        .WithDescription("Ainda não iniciada")
                        .WithDueDate(DateTime.UtcNow.AddDays(2))
                        .WithPriority(ActivityPriority.Medium)
                        .WithStatus(ActivityStatus.Pending)
                        .WithProject(project)
                        .Build(),

                    new ActivityBuilder()
                        .WithTitle("Tarefa em andamento")
                        .WithDescription("Executando...")
                        .WithDueDate(DateTime.UtcNow.AddDays(5))
                        .WithPriority(ActivityPriority.High)
                        .WithStatus(ActivityStatus.InProgress)
                        .WithProject(project)
                        .Build(),

                    new ActivityBuilder()
                        .WithTitle("Tarefa concluída")
                        .WithDescription("Já finalizada")
                        .WithDueDate(DateTime.UtcNow.AddDays(-1))
                        .WithPriority(ActivityPriority.Low)
                        .WithStatus(ActivityStatus.Completed)
                        .WithProject(project)
                        .Build()
                };

                context.Users.Add(user);
                context.Projects.Add(project);
                context.Activities.AddRange(activities);

                await context.SaveChangesAsync();
            }
        }
    }
}
