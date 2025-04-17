using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities
{
    public class Activity
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime DueDate { get; private set; }
        public ActivityStatus Status { get; set; }
        public ActivityPriority Priority { get; private set; }

        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }

        public Activity(string title, string description, DateTime dueDate, ActivityPriority priority)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
            Status = ActivityStatus.Pending;
        }

        public void UpdateDetails(string title, string description, DateTime dueDate, ActivityStatus status)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            Status = status;
        }
    }
}
