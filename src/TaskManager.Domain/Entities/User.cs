namespace TaskManager.Domain.Entities
{
    public class User : BaseEntity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }

        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
