namespace TaskManager.Application.DTOs
{
    public class ProjectCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
    }
}
