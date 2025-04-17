using System.ComponentModel.DataAnnotations;
using TaskManager.Domain.Enums;
using DomainTaskStatus = TaskManager.Domain.Enums.ActivityStatus;

namespace TaskManager.Application.DTOs.Task
{
    public class CreateActivityRequest
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required DateTime DueDate { get; set; }
        public DomainTaskStatus Status { get; set; } = DomainTaskStatus.Pending;
        public required Guid ProjectId { get; set; }

        [EnumDataType(typeof(ActivityPriority))]
        public required ActivityPriority Priority { get; set; }
    }
}
