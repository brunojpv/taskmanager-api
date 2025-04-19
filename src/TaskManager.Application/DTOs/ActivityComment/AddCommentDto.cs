namespace TaskManager.Application.DTOs.ActivityComment
{
    public class AddCommentDto
    {
        public Guid ActivityId { get; set; }
        public string Content { get; set; } = "";
    }
}
