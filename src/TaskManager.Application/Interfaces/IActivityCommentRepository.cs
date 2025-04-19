using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface IActivityCommentRepository
    {
        Task AddAsync(ActivityComment comment);
        Task<List<ActivityComment>> GetByActivityIdAsync(Guid activityId);
    }
}
