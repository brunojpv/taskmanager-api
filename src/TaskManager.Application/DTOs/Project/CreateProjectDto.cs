namespace TaskManager.Application.DTOs.Project
{
    public class CreateProjectDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required Guid UserId { get; set; }
    }
}
