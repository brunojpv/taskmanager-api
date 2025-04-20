using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities
{
    public class Activity : BaseEntity
    {
        public string? Title { get; private set; }
        public string? Description { get; private set; }
        public DateTime DueDate { get; private set; }
        public ActivityStatus Status { get; set; }
        public ActivityPriority Priority { get; private set; }
        public Guid ProjectId { get; set; }

        public Project? Project { get; set; }

        public ICollection<ActivityHistory> ActivityHistories { get; private set; } = new List<ActivityHistory>();

        public ICollection<ActivityComment> ActivityComments { get; private set; } = new List<ActivityComment>();

        public Activity(string? title, string? description, DateTime dueDate, ActivityPriority priority, Guid projectId)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
            Status = ActivityStatus.Pending;
            ProjectId = projectId;
        }

        protected Activity() { }

        public void UpdateDetails(string? title, string? description, DateTime dueDate, ActivityStatus status, Guid projectId)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            Status = status;
            ProjectId = projectId;
        }
    }
}
