namespace TaskManager.Domain.Entities
{
    public class ActivityComment : BaseEntity
    {
        public Guid ActivityId { get; private set; }
        public Guid UserId { get; private set; }
        public string Content { get; private set; } = string.Empty;

        public Activity? Activity { get; set; }
        public User? User { get; set; }

        protected ActivityComment() { }

        public ActivityComment(Guid activityId, Guid userId, string content)
        {
            if (activityId == Guid.Empty)
                throw new ArgumentException("ActivityId não pode ser vazio.", nameof(activityId));

            if (userId == Guid.Empty)
                throw new ArgumentException("UserId não pode ser vazio.", nameof(userId));

            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Comentário não pode ser vazio ou nulo.", nameof(content));

            ActivityId = activityId;
            UserId = userId;
            Content = content;
        }
    }
}
