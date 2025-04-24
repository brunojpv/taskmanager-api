using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Infrastructure.Data.Seed
{
    /// <summary>
    /// Fábrica de dados padrão utilizada para popular o banco em ambiente de desenvolvimento/testes.
    /// </summary>
    public class SeedFactory
    {
        private readonly TaskManagerDbContext _dbContext;

        public SeedFactory(TaskManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateUserAsync(string name = "Usuário Teste", string email = null, bool isManager = false)
        {
            email ??= $"user_{Guid.NewGuid()}@teste.com";

            var user = new User(name, email, isManager);

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<Project> CreateProjectAsync(string name = "Projeto Teste", string description = "Descrição do projeto de teste", Guid? userId = null)
        {
            if (userId == null)
            {
                var user = await CreateUserAsync();
                userId = user.Id;
            }

            var project = new Project(name, description, userId.Value);

            await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync();

            return project;
        }

        public async Task<TaskItem> CreateTaskAsync(
            string title = "Tarefa Teste",
            string description = "Descrição da tarefa de teste",
            DateTime? dueDate = null,
            TaskPriority priority = TaskPriority.Medium,
            Guid? projectId = null,
            TaskItemStatus status = TaskItemStatus.Pending)
        {
            if (projectId == null)
            {
                var project = await CreateProjectAsync();
                projectId = project.Id;
            }

            dueDate ??= DateTime.UtcNow.AddDays(7);

            var task = new TaskItem(title, description, dueDate, priority, projectId.Value);

            if (status != TaskItemStatus.Pending)
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync();
                if (user == null)
                {
                    user = await CreateUserAsync();
                }

                task.UpdateStatus(status, user.Id);
            }

            await _dbContext.Tasks.AddAsync(task);
            await _dbContext.SaveChangesAsync();

            return task;
        }

        public async Task<TaskComment> CreateTaskCommentAsync(string content = "Comentário de teste", Guid? taskId = null, Guid? userId = null)
        {
            if (taskId == null)
            {
                var taskItem = await CreateTaskAsync();
                taskId = taskItem.Id;
            }

            if (userId == null)
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync();
                if (user == null)
                {
                    user = await CreateUserAsync();
                }
                userId = user.Id;
            }

            var task = await _dbContext.Tasks.FindAsync(taskId.Value);
            if (task == null)
            {
                throw new InvalidOperationException("Tarefa não encontrada");
            }

            task.AddComment(content, userId.Value);

            await _dbContext.SaveChangesAsync();

            return task.Comments.LastOrDefault();
        }

        public async Task CreateSampleDataAsync(int userCount = 5, int projectsPerUser = 2, int tasksPerProject = 3)
        {
            var users = new List<User>();
            for (int i = 0; i < userCount; i++)
            {
                var isManager = i == 0;
                var user = await CreateUserAsync($"Usuário {i + 1}", $"usuario{i + 1}@exemplo.com", isManager);
                users.Add(user);
            }

            foreach (var user in users)
            {
                for (int i = 0; i < projectsPerUser; i++)
                {
                    var project = await CreateProjectAsync($"Projeto {i + 1} de {user.Name}", $"Descrição do projeto {i + 1}", user.Id);

                    for (int j = 0; j < tasksPerProject; j++)
                    {
                        var priority = (TaskPriority)(j % 3);
                        var status = (TaskItemStatus)(j % 3);

                        var task = await CreateTaskAsync(
                            $"Tarefa {j + 1} do {project.Name}",
                            $"Descrição da tarefa {j + 1}",
                            DateTime.UtcNow.AddDays(j + 1),
                            priority,
                            project.Id,
                            status
                        );

                        await CreateTaskCommentAsync($"Comentário para a {task.Title}", task.Id, user.Id);
                    }
                }
            }
        }

        public async Task CleanDatabaseAsync()
        {
            _dbContext.TaskComments.RemoveRange(_dbContext.TaskComments);
            _dbContext.TaskHistoryEntries.RemoveRange(_dbContext.TaskHistoryEntries);
            _dbContext.Tasks.RemoveRange(_dbContext.Tasks);
            _dbContext.Projects.RemoveRange(_dbContext.Projects);
            _dbContext.Users.RemoveRange(_dbContext.Users);

            await _dbContext.SaveChangesAsync();
        }
    }
}
