using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Infrastructure.Data
{
    public static class SeedFactory
    {
        public static User CreateDefaultUser()
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("123456");
            return User.Create("Bruno Dev", "bruno@dev.com", hashedPassword);
        }

        public static List<User> CreateUserProjectActivitySeed()
        {
            var users = new List<User>();

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("123456");
            var user = User.Create("Bruno Dev", "bruno@dev.com", hashedPassword);

            var project = new Project(
                name: "Projeto Principal",
                description: "Projeto com tarefas, comentários e histórico",
                userId: user.Id
            );

            project.User = user;

            var activityA = new Activity(
                title: "Atividade A",
                description: "Primeira atividade",
                dueDate: DateTime.UtcNow.AddDays(1),
                priority: ActivityPriority.Medium,
                projectId: project.Id
            )
            { Project = project, Status = ActivityStatus.Pending };

            var activityB = new Activity(
                title: "Atividade B",
                description: "Segunda atividade",
                dueDate: DateTime.UtcNow.AddDays(3),
                priority: ActivityPriority.High,
                projectId: project.Id
            )
            { Project = project, Status = ActivityStatus.InProgress };

            var comment1 = new ActivityComment(activityA.Id, user.Id, "Comentário sobre a atividade A");
            var comment2 = new ActivityComment(activityB.Id, user.Id, "Comentário sobre a atividade B");

            var history1 = new ActivityHistory(activityA.Id, "Atividade criada.", user.Id);
            var history2 = new ActivityHistory(activityB.Id, "Atividade em andamento.", user.Id);

            activityA.ActivityComments.Add(comment1);
            activityA.ActivityHistories.Add(history1);

            activityB.ActivityComments.Add(comment2);
            activityB.ActivityHistories.Add(history2);

            project.AddActivity(activityA);
            project.AddActivity(activityB);

            user.AddProject(project);
            users.Add(user);

            return users;
        }

        public static Project CreateProject(User user)
        {
            var project = new Project(
                name: "Projeto de Teste",
                description: "Criado automaticamente via seed",
                userId: user.Id
            );

            project.User = user;
            return project;
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
