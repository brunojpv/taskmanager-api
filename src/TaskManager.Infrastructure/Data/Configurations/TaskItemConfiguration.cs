using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Infrastructure.Data.Configurations
{
    public class TaskItemConfiguration : BaseEntityConfiguration<TaskItem>
    {
        public override void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Title).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(500);
            builder.Property(e => e.Status).IsRequired()
                  .HasConversion(
                      v => v.ToString(),
                      v => (TaskItemStatus)Enum.Parse(typeof(TaskItemStatus), v));
            builder.Property(e => e.Priority).IsRequired()
                  .HasConversion(
                      v => v.ToString(),
                      v => (TaskPriority)Enum.Parse(typeof(TaskPriority), v));

            builder.HasMany(e => e.History)
                  .WithOne(e => e.Task)
                  .HasForeignKey(e => e.TaskId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Comments)
                  .WithOne(e => e.Task)
                  .HasForeignKey(e => e.TaskId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
