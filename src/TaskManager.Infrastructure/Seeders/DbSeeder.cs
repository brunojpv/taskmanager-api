using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Seeders
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
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

                var task = new TaskItem
                {
                    Title = "Tarefa Inicial",
                    Description = "Criada pelo seed",
                    DueDate = DateTime.UtcNow.AddDays(7),
                    Status = TaskStatus.Pending,
                    Project = project
                };

                context.Users.Add(user);
                context.Projects.Add(project);
                context.Tasks.Add(task);

                await context.SaveChangesAsync();
            }
        }
    }
}
