using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Data.Configurations
{
    public class ActivityHistoryConfiguration : IEntityTypeConfiguration<ActivityHistory>
    {
        public void Configure(EntityTypeBuilder<ActivityHistory> builder)
        {
            builder.HasKey(ah => ah.Id);

            builder.Property(ah => ah.ChangeDescription)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(ah => ah.ChangedByUserId)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(ah => ah.Activity)
                .WithMany(a => a.ActivityHistories)
                .HasForeignKey(ah => ah.ActivityId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
