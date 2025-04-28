using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Repositories
{
    public interface ITaskRepository
    {
        Task<List<TaskItem>> GetAllByProjectIdAsync(Guid projectId);
        Task<TaskItem> GetByIdAsync(Guid id);
        Task<TaskItem> AddAsync(TaskItem task);
        Task UpdateAsync(TaskItem task);
        Task<bool> DeleteAsync(TaskItem task);
        Task<bool> ExistsAsync(Guid id);
        Task<int> GetCompletedTasksCountByUserIdLastDaysAsync(Guid userId, int days);
    }
}
