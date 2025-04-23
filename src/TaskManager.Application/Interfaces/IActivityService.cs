using TaskManager.Application.DTOs.Activity;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface IActivityService
    {
        Task<IEnumerable<Activity>> GetAllAsync(Guid? userId);
        Task<Activity?> GetByIdAsync(Guid id);
        Task AddAsync(Activity activity);
        Task UpdateAsync(Activity activity);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<ActivityDto>> GetAllActivityAsync(Guid? userId);
        Task<ActivityDto> CreateActivityAsync(CreateActivityDto dto);
        Task<ActivityDto?> UpdateActivityAsync(Guid activityId, UpdateActivityDto dto, Guid? userId);
        Task<bool> DeleteActivityAsync(Guid activityId, Guid? userId);
    }
}
