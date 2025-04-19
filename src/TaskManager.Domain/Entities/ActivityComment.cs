namespace TaskManager.Domain.Entities
{
    public class ActivityComment : BaseEntity
    {
        public Guid ActivityId { get; private set; }
        public Guid UserId { get; private set; }
        public string Content { get; private set; } = "";

        public Activity Activity { get; private set; }
        public User User { get; private set; }

        public ActivityComment(Guid activityId, Guid userId, string content)
        {
            ActivityId = activityId;
            UserId = userId;
            Content = content;
        }

        protected ActivityComment() { }
    }
}
