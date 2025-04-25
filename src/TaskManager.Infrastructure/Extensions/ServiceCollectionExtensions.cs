using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Services;
using TaskManager.Domain.Repositories;
using TaskManager.Infrastructure.Data;
using TaskManager.Infrastructure.Data.Seed;
using TaskManager.Infrastructure.Repositories;

namespace TaskManager.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTaskManagerServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Registra o DbContext
            services.AddDbContext<TaskManagerDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Registra repositórios
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITaskHistoryRepository, TaskHistoryRepository>();
            services.AddScoped<ITaskCommentRepository, TaskCommentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IReportRepository, ReportRepository>(); // Adicionado o novo repositório

            // Registra serviços
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IReportService, ReportService>();

            // Registra SeedFactory
            services.AddScoped<SeedFactory>();

            return services;
        }

        public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TaskManagerDbContext>();

            // Aplica todas as migrações pendentes
            await dbContext.Database.MigrateAsync();

            // Verifica se já existem dados no banco
            bool databaseIsEmpty = !await dbContext.Users.AnyAsync();

            if (databaseIsEmpty)
            {
                // Seed inicial usando SeedFactory
                var seedFactory = scope.ServiceProvider.GetRequiredService<SeedFactory>();
                await seedFactory.CreateSampleDataAsync();
            }
        }
    }
}
