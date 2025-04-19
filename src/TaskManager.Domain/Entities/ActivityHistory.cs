namespace TaskManager.Domain.Entities
{
    public class ActivityHistory : BaseEntity
    {
        public Guid ActivityId { get; private set; }
        public Guid ChangedByUserId { get; private set; }
        public string ChangeDescription { get; private set; }

        public Activity? Activity { get; private set; }

        public ActivityHistory(Guid activityId, Guid changedByUserId, string changeDescription)
        {
            ActivityId = activityId;
            ChangedByUserId = changedByUserId;
            ChangeDescription = changeDescription;
        }

        private ActivityHistory() { }
    }
}
