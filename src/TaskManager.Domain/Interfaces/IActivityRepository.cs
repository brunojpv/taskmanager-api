using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface IActivityRepository
    {
        Task<IEnumerable<Activity>> GetAllAsync(Guid userId);
        Task<Activity?> GetByIdAsync(Guid id);
        Task AddAsync(Activity task);
        Task UpdateAsync(Activity task);
        Task DeleteAsync(Guid id);
    }
}
