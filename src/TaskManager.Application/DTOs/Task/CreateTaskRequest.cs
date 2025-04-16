using DomainTaskStatus = TaskManager.Domain.Entities.TaskStatus;

namespace TaskManager.Application.DTOs.Task
{
    public class CreateTaskRequest
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DomainTaskStatus Status { get; set; } = DomainTaskStatus.Pending;
        public required Guid ProjectId { get; set; }
    }
}
