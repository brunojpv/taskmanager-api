using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Data.Configurations
{
    public class TaskHistoryEntryConfiguration : BaseEntityConfiguration<TaskHistoryEntry>
    {
        public override void Configure(EntityTypeBuilder<TaskHistoryEntry> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Action).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Details).HasMaxLength(500);
            builder.Property(e => e.Timestamp).IsRequired();

            builder.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.SetNull)
                  .IsRequired(false);
        }
    }
}
