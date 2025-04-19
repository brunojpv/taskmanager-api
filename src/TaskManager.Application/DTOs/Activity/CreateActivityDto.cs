using System.ComponentModel.DataAnnotations;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs.Activity
{
    public class CreateActivityDto
    {
        public required string Title { get; set; }

        public required string Description { get; set; }

        public required DateTime DueDate { get; set; }

        [EnumDataType(typeof(ActivityPriority))]
        public required ActivityPriority Priority { get; set; }

        public required Guid ProjectId { get; set; }
    }
}
