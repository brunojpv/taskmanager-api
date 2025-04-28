using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(new DateTimeKindConverter());
                    }
                }
            }
        }

        public class DateTimeKindConverter : ValueConverter<DateTime, DateTime>
        {
            public DateTimeKindConverter() : base(
                d => d.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(d, DateTimeKind.Utc) : d,
                d => DateTime.SpecifyKind(d, DateTimeKind.Utc))
            {
            }
        }
    }
}
