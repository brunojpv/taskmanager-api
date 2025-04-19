using TaskManager.Application.DTOs.ActivityComment;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface IActivityCommentService
    {
        Task AddCommentAsync(Guid userId, AddCommentDto dto);
        Task<List<ActivityComment>> GetCommentsByActivityIdAsync(Guid activityId);
    }
}
