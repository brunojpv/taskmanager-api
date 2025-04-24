using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Repositories
{
    public interface ITaskHistoryRepository
    {
        Task<List<TaskHistoryEntry>> GetAllByTaskIdAsync(Guid taskId);
        Task<TaskHistoryEntry> AddAsync(TaskHistoryEntry historyEntry);
    }
}
