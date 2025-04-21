using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities
{
    public class Activity : BaseEntity
    {
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public DateTime DueDate { get; private set; }
        public ActivityStatus Status { get; set; }
        public ActivityPriority Priority { get; private set; }
        public Guid ProjectId { get; private set; }

        public Project? Project { get; set; }

        public ICollection<ActivityHistory> ActivityHistories { get; private set; } = new List<ActivityHistory>();
        public ICollection<ActivityComment> ActivityComments { get; private set; } = new List<ActivityComment>();

        protected Activity() { }

        public Activity(string title, string description, DateTime dueDate, ActivityPriority priority, Guid projectId)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Título da atividade não pode ser vazio ou nulo.", nameof(title));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Descrição da atividade não pode ser vazia ou nula.", nameof(description));

            if (projectId == Guid.Empty)
                throw new ArgumentException("ProjectId não pode ser vazio.", nameof(projectId));

            Title = title;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
            Status = ActivityStatus.Pending;
            ProjectId = projectId;
        }

        public void UpdateDetails(string title, string description, DateTime dueDate, ActivityStatus status, Guid projectId)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Título da atividade não pode ser vazio ou nulo.", nameof(title));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Descrição da atividade não pode ser vazia ou nula.", nameof(description));

            if (projectId == Guid.Empty)
                throw new ArgumentException("ProjectId não pode ser vazio.", nameof(projectId));

            Title = title;
            Description = description;
            DueDate = dueDate;
            Status = status;
            ProjectId = projectId;
        }
    }
}
