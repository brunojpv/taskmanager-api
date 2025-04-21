namespace TaskManager.Domain.Entities
{
    public class ActivityHistory : BaseEntity
    {
        public Guid ActivityId { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public Guid UserId { get; private set; }

        public Activity? Activity { get; set; }
        public User? User { get; set; }

        protected ActivityHistory() { }

        public ActivityHistory(Guid activityId, string description, Guid userId)
        {
            if (activityId == Guid.Empty)
                throw new ArgumentException("ActivityId não pode ser vazio.", nameof(activityId));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Descrição do histórico não pode ser vazia ou nula.", nameof(description));

            if (userId == Guid.Empty)
                throw new ArgumentException("UserId não pode ser vazio.", nameof(userId));

            ActivityId = activityId;
            Description = description;
            UserId = userId;
        }
    }
}
