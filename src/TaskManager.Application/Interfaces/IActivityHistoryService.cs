using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface IActivityHistoryService
    {
        Task RecordHistoryAsync(Guid activityId, Guid userId, string description);
        Task<IEnumerable<ActivityHistory>> GetHistoryByActivityIdAsync(Guid activityId);
    }
}
