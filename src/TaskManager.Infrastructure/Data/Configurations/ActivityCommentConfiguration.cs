using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Data.Configurations
{
    public class ActivityCommentConfiguration : IEntityTypeConfiguration<ActivityComment>
    {
        public void Configure(EntityTypeBuilder<ActivityComment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Content)
                .HasMaxLength(1000)
                .IsRequired();

            builder.HasOne(c => c.Activity)
                .WithMany(a => a.ActivityComments)
                .HasForeignKey(c => c.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
