using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Infrastructure.Data
{
    /// <summary>
    /// Fábrica de dados padrão utilizada para popular o banco em ambiente de desenvolvimento/testes.
    /// </summary>
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
            var user = CreateDefaultUser();

            var project = new Project(
                name: "Projeto Principal",
                description: "Projeto com tarefas, comentários e histórico",
                userId: user.Id
            );

            project.User = user;

            var activityA = Activity.Create(
                title: "Atividade A",
                description: "Primeira atividade",
                dueDate: DateTime.UtcNow.AddDays(1),
                priority: ActivityPriority.Medium,
                projectId: project.Id
            );

            var activityB = Activity.Create(
                title: "Atividade B",
                description: "Segunda atividade",
                dueDate: DateTime.UtcNow.AddDays(3),
                priority: ActivityPriority.High,
                projectId: project.Id
            );

            activityA.SetProject(project);
            activityB.SetProject(project);

            activityA.SetStatus(ActivityStatus.Pending);
            activityB.SetStatus(ActivityStatus.InProgress);

            activityA.ActivityComments.Add(new ActivityComment(activityA.Id, user.Id, "Comentário sobre a atividade A"));
            activityB.ActivityComments.Add(new ActivityComment(activityB.Id, user.Id, "Comentário sobre a atividade B"));

            activityA.ActivityHistories.Add(new ActivityHistory(activityA.Id, "Atividade criada.", user.Id));
            activityB.ActivityHistories.Add(new ActivityHistory(activityB.Id, "Atividade em andamento.", user.Id));

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
            var activities = new List<Activity>
            {
                Activity.Create("Tarefa pendente", "Ainda não iniciada", DateTime.UtcNow.AddDays(2), ActivityPriority.Medium, project.Id),
                Activity.Create("Tarefa em andamento", "Executando...", DateTime.UtcNow.AddDays(5), ActivityPriority.High, project.Id),
                Activity.Create("Tarefa concluída", "Já finalizada", DateTime.UtcNow.AddDays(-1), ActivityPriority.Low, project.Id)
            };

            activities[0].SetStatus(ActivityStatus.Pending);
            activities[1].SetStatus(ActivityStatus.InProgress);
            activities[2].SetStatus(ActivityStatus.Completed);

            foreach (var activity in activities)
                activity.SetProject(project);

            return activities;
        }
    }
}
