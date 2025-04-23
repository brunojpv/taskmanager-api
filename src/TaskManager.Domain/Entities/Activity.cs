using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities
{
    public class Activity : BaseEntity
    {
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public DateTime DueDate { get; private set; }
        public ActivityStatus Status { get; private set; }
        public ActivityPriority Priority { get; private set; }
        public Guid ProjectId { get; private set; }

        public Project? Project { get; private set; }

        public ICollection<ActivityHistory> ActivityHistories { get; private set; } = new List<ActivityHistory>();
        public ICollection<ActivityComment> ActivityComments { get; private set; } = new List<ActivityComment>();

        public void SetStatus(ActivityStatus status) => Status = status;
        public void SetProject(Project project) => Project = project;

        protected Activity() { }

        private Activity(string title, string description, DateTime dueDate, ActivityPriority priority, Guid projectId)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
            Status = ActivityStatus.Pending;
            ProjectId = projectId;
        }

        public static Activity Create(string title, string? description, DateTime dueDate, ActivityPriority priority, Guid projectId)
        {
            ValidateInputs(title, description, priority, projectId);

            return new Activity(title!, description!, dueDate, priority, projectId);
        }

        public void Update(string title, string? description, DateTime dueDate, ActivityStatus status, ActivityPriority priority)
        {
            ValidateInputs(title, description, priority);

            if (!Enum.IsDefined(typeof(ActivityStatus), status))
                throw new ArgumentException("Status inválido.", nameof(status));

            Title = title!;
            Description = description!;
            DueDate = dueDate;
            Status = status;
            Priority = priority;
        }

        private static void ValidateInputs(string? title, string? description, ActivityPriority priority, Guid? projectId = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("O título da atividade é obrigatório.", nameof(title));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("A descrição da atividade é obrigatória.", nameof(description));

            if (!Enum.IsDefined(typeof(ActivityPriority), priority))
                throw new ArgumentException("Prioridade inválida.", nameof(priority));

            if (projectId.HasValue && projectId.Value == Guid.Empty)
                throw new ArgumentException("O ID do projeto é inválido.", nameof(projectId));
        }
    }
}
