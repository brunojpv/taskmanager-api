namespace TaskManager.Application.DTOs
{
    public class TaskHistoryDTO
    {
        public Guid Id { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
    }
}
