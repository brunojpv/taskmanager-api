using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Data.Configurations
{
    public class TaskCommentConfiguration : BaseEntityConfiguration<TaskComment>
    {
        public override void Configure(EntityTypeBuilder<TaskComment> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Content).IsRequired().HasMaxLength(500);

            builder.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
