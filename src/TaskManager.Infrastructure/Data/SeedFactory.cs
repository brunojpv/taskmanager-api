using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Infrastructure.Data
{
    public static class SeedFactory
    {
        public static User CreateDefaultUser()
        {
            return new User
            {
                Name = "Bruno Dev",
                Email = "bruno@dev.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456")
            };
        }

        public static Project CreateProject(User user)
        {
            return new Project
            {
                Name = "Projeto de Teste",
                Description = "Criado automaticamente via seed",
                User = user
            };
        }

        public static List<Activity> CreateSampleActivities(Project project)
        {
            return new List<Activity>
            {
                new Activity(
                    title: "Tarefa pendente",
                    description: "Ainda não iniciada",
                    dueDate: DateTime.UtcNow.AddDays(2),
                    priority: ActivityPriority.Medium,
                    projectId: project.Id
                ) { Status = ActivityStatus.Pending, Project = project },

                new Activity(
                    title: "Tarefa em andamento",
                    description: "Executando...",
                    dueDate: DateTime.UtcNow.AddDays(5),
                    priority: ActivityPriority.High,
                    projectId: project.Id
                ) { Status = ActivityStatus.InProgress, Project = project },

                new Activity(
                    title: "Tarefa concluída",
                    description: "Já finalizada",
                    dueDate: DateTime.UtcNow.AddDays(-1),
                    priority: ActivityPriority.Low,
                    projectId: project.Id
                ) { Status = ActivityStatus.Completed, Project = project }
            };
        }
    }
}
