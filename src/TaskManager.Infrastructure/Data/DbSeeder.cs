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

                var activity1 = new ActivityBuilder()
                    .WithTitle("Tarefa pendente")
                    .WithDescription("Ainda não iniciada")
                    .WithDueDate(DateTime.UtcNow.AddDays(2))
                    .WithPriority(ActivityPriority.Medium)
                    .WithStatus(ActivityStatus.Pending)
                    .WithProject(project)
                    .Build();

                var activity2 = new ActivityBuilder()
                    .WithTitle("Tarefa em andamento")
                    .WithDescription("Executando...")
                    .WithDueDate(DateTime.UtcNow.AddDays(5))
                    .WithPriority(ActivityPriority.High)
                    .WithStatus(ActivityStatus.InProgress)
                    .WithProject(project)
                    .Build();

                var activity3 = new ActivityBuilder()
                    .WithTitle("Tarefa concluída")
                    .WithDescription("Já finalizada")
                    .WithDueDate(DateTime.UtcNow.AddDays(-1))
                    .WithPriority(ActivityPriority.Low)
                    .WithStatus(ActivityStatus.Completed)
                    .WithProject(project)
                    .Build();

                // Comentários e histórico
                activity1.ActivityComments.Add(new ActivityComment(activity1.Id, user.Id, "Comentário da atividade pendente"));
                activity2.ActivityComments.Add(new ActivityComment(activity2.Id, user.Id, "Comentário da atividade em andamento"));
                activity3.ActivityComments.Add(new ActivityComment(activity3.Id, user.Id, "Comentário da atividade finalizada"));

                activity1.ActivityHistories.Add(new ActivityHistory(activity1.Id, "Atividade criada", user.Id));
                activity2.ActivityHistories.Add(new ActivityHistory(activity2.Id, "Atividade iniciada", user.Id));
                activity3.ActivityHistories.Add(new ActivityHistory(activity3.Id, "Atividade concluída", user.Id));

                // Adicionar atividades ao projeto
                project.AddActivity(activity1);
                project.AddActivity(activity2);
                project.AddActivity(activity3);

                // Adicionar projeto ao usuário
                user.AddProject(project);

                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
        }
    }
}
