using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Data.Configurations;
using TaskManager.Infrastructure.Data.Seed;

namespace TaskManager.Infrastructure.Data
{
    public class TaskManagerDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<TaskHistoryEntry> TaskHistoryEntries { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }

        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new TaskItemConfiguration());
            modelBuilder.ApplyConfiguration(new TaskHistoryEntryConfiguration());
            modelBuilder.ApplyConfiguration(new TaskCommentConfiguration());

            DatabaseSeed.SeedData(modelBuilder);
        }
    }
}
