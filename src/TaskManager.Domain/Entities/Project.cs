namespace TaskManager.Domain.Entities
{
    public class Project : BaseEntity
    {
        public const int MaxActivities = 20;

        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid UserId { get; set; }

        public User? User { get; set; }

        public ICollection<Activity> Activities { get; set; } = new List<Activity>();

        public Project(string? name, string? description, Guid userId)
        {
            Name = name;
            Description = description;
            UserId = userId;
        }

        protected Project() { }

        public void UpdateDetails(string? name, string? description, Guid userId)
        {
            Name = name;
            Description = description;
            UserId = userId;
        }

        public bool CanAddNewActivity() => Activities.Count < MaxActivities;
    }
}
