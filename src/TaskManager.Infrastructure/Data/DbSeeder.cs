using TaskManager.Domain.Entities;
using DomainTaskStatus = TaskManager.Domain.Enums.ActivityStatus;

namespace TaskManager.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async System.Threading.Tasks.Task SeedAsync(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                var user = new User
                {
                    Name = "Bruno Dev",
                    Email = "bruno@dev.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456")
                };

                var project = new Project
                {
                    Name = "Projeto de Teste",
                    Description = "Criado automaticamente via seed",
                    User = user
                };

                var tasks = new List<Task>
            {
                new TaskItem
                {
                    Title = "Tarefa pendente",
                    Description = "Ainda não iniciada",
                    DueDate = DateTime.UtcNow.AddDays(2),
                    Status = DomainTaskStatus.Pending,
                    Project = project
                },
                new TaskItem
                {
                    Title = "Tarefa em andamento",
                    Description = "Executando...",
                    DueDate = DateTime.UtcNow.AddDays(5),
                    Status = DomainTaskStatus.InProgress,
                    Project = project
                },
                new TaskItem
                {
                    Title = "Tarefa concluída",
                    Description = "Já finalizada",
                    DueDate = DateTime.UtcNow.AddDays(-1),
                    Status = DomainTaskStatus.Completed,
                    Project = project
                }
            };

                context.Users.Add(user);
                context.Projects.Add(project);
                context.Tasks.AddRange(tasks);

                await context.SaveChangesAsync();
            }
        }
    }
}
