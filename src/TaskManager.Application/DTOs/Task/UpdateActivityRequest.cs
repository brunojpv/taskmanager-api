using DomainTaskStatus = TaskManager.Domain.Enums.ActivityStatus;

namespace TaskManager.Application.DTOs.Task
{
    public class UpdateActivityRequest
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DomainTaskStatus Status { get; set; }
        public required Guid ProjectId { get; set; }
    }
}
