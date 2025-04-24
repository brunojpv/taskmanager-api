using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs
{
    public class TaskUpdateDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskItemStatus Status { get; set; }
        public Guid UserId { get; set; }
    }
}
