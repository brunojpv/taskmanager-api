namespace TaskManager.Domain.Entities
{
    public class Project : BaseEntity
    {
        public const int MaxActivities = 20;

        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public Guid UserId { get; private set; }

        public User? User { get; set; }

        private readonly List<Activity> _activities = new();
        public IReadOnlyCollection<Activity> Activities => _activities;

        protected Project() { }

        public Project(string name, string description, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome do projeto não pode ser vazio ou nulo.", nameof(name));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Descrição do projeto não pode ser vazia ou nula.", nameof(description));

            if (userId == Guid.Empty)
                throw new ArgumentException("UserId não pode ser vazio.", nameof(userId));

            Name = name;
            Description = description;
            UserId = userId;
        }

        public void UpdateDetails(string name, string description, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome do projeto não pode ser vazio ou nulo.", nameof(name));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Descrição do projeto não pode ser vazia ou nula.", nameof(description));

            if (userId == Guid.Empty)
                throw new ArgumentException("UserId não pode ser vazio.", nameof(userId));

            Name = name;
            Description = description;
            UserId = userId;
        }

        public void AddActivity(Activity activity)
        {
            _activities.Add(activity);
        }

        public bool CanAddNewActivity() => _activities.Count < MaxActivities;
    }
}
