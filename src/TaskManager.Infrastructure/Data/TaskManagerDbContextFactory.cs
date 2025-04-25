using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TaskManager.Infrastructure.Data
{
    public class TaskManagerDbContextFactory : IDesignTimeDbContextFactory<TaskManagerDbContext>
    {
        public TaskManagerDbContext CreateDbContext(string[] args)
        {
            string apiProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "TaskManager.Api");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(apiProjectPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            var builder = new DbContextOptionsBuilder<TaskManagerDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseNpgsql(connectionString, b => b.MigrationsAssembly("TaskManager.Infrastructure"));

            return new TaskManagerDbContext(builder.Options);
        }
    }
}
