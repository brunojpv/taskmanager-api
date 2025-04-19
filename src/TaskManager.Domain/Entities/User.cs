using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities
{
    public class User : BaseEntity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public UserRole Role { get; set; } = UserRole.Regular;

        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
