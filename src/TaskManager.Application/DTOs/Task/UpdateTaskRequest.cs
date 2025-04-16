using DomainTaskStatus = TaskManager.Domain.Entities.TaskStatus;

namespace TaskManager.Application.DTOs.Task
{
    public class UpdateTaskRequest
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DomainTaskStatus Status { get; set; }
        public required Guid ProjectId { get; set; }
    }
}
