using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface IActivityHistoryRepository
    {
        Task AddAsync(ActivityHistory history);
        Task<IEnumerable<ActivityHistory>> GetByActivityIdAsync(Guid activityId);
    }
}
