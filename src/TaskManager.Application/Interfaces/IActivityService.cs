using TaskManager.Application.DTOs.Activity;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface IActivityService
    {
        Task<IEnumerable<Activity>> GetAllAsync(Guid userId);
        Task<Activity?> GetByIdAsync(Guid id);
        Task AddAsync(Activity task);
        Task UpdateAsync(Activity task);
        Task DeleteAsync(Guid id);
        Task<Activity> CreateActivityAsync(CreateActivityDto dto);
        Task UpdateActivityAsync(Guid activityId, UpdateActivityDto dto, Guid userId);
    }
}
