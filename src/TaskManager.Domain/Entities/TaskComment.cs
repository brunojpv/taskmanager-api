namespace TaskManager.Domain.Entities
{
    public class TaskComment : BaseEntity
    {
        public string Content { get; private set; }
        public Guid TaskId { get; private set; }
        public TaskItem Task { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public TaskComment(string content, Guid taskId, Guid userId)
        {
            Content = content;
            TaskId = taskId;
            UserId = userId;
        }

        public void Update(string content)
        {
            Content = content;
            SetUpdated();
        }

        private TaskComment() { }
    }
}
