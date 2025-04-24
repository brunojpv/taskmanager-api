namespace TaskManager.Domain.Entities
{
    public class TaskHistoryEntry : BaseEntity
    {
        public string Action { get; private set; }
        public string Details { get; private set; }
        public DateTime Timestamp { get; private set; }
        public Guid TaskId { get; private set; }
        public TaskItem Task { get; private set; }
        public Guid? UserId { get; private set; }
        public User User { get; private set; }

        public TaskHistoryEntry(string action, string details, Guid taskId, Guid? userId)
        {
            Action = action;
            Details = details;
            Timestamp = DateTime.UtcNow;
            TaskId = taskId;
            UserId = userId;
        }

        private TaskHistoryEntry() { }
    }
}
