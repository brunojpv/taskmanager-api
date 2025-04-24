namespace TaskManager.Application.DTOs
{
    public class TaskCommentCreateDTO
    {
        public string Content { get; set; }
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
    }
}
