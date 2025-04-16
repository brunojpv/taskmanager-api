namespace TaskManager.Application.DTOs.Project
{
    public class UpdateProjectRequest
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
