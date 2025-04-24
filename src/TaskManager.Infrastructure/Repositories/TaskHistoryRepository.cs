using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class TaskHistoryRepository : ITaskHistoryRepository
    {
        private readonly TaskManagerDbContext _dbContext;

        public TaskHistoryRepository(TaskManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TaskHistoryEntry>> GetAllByTaskIdAsync(Guid taskId)
        {
            return await _dbContext.TaskHistoryEntries
                .Where(h => h.TaskId == taskId)
                .OrderByDescending(h => h.Timestamp)
                .ToListAsync();
        }

        public async Task<TaskHistoryEntry> AddAsync(TaskHistoryEntry historyEntry)
        {
            await _dbContext.TaskHistoryEntries.AddAsync(historyEntry);
            await _dbContext.SaveChangesAsync();
            return historyEntry;
        }
    }
}
