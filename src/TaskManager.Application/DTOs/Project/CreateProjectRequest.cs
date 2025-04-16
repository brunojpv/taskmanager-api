namespace TaskManager.Application.DTOs.Project
{
    public class CreateProjectRequest
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
