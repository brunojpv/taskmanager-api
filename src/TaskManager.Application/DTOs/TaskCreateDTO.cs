using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs
{
    public class TaskCreateDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public Guid ProjectId { get; set; }
    }
}
