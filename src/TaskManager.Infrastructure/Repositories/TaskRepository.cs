using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagerDbContext _dbContext;

        public TaskRepository(TaskManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TaskItem>> GetAllByProjectIdAsync(Guid projectId)
        {
            return await _dbContext.Tasks
                .Include(t => t.Comments)
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<TaskItem> GetByIdAsync(Guid id)
        {
            return await _dbContext.Tasks
                .Include(t => t.History)
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaskItem> AddAsync(TaskItem task)
        {
            await _dbContext.Tasks.AddAsync(task);
            await _dbContext.SaveChangesAsync();
            return task;
        }

        public async Task UpdateAsync(TaskItem task)
        {
            _dbContext.Tasks.Update(task);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var task = await _dbContext.Tasks.FindAsync(id);
            if (task == null)
            {
                return false;
            }

            _dbContext.Tasks.Remove(task);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbContext.Tasks.AnyAsync(t => t.Id == id);
        }

        public async Task<int> GetCompletedTasksCountByUserIdLastDaysAsync(Guid userId, int days)
        {
            var fromDate = DateTime.UtcNow.AddDays(-days);

            return await _dbContext.Tasks
                .Include(t => t.Project)
                .Where(t => t.Project.UserId == userId &&
                       t.Status == TaskItemStatus.Completed &&
                       t.History.Any(h => h.Action == "Status alterado" &&
                                    h.Details.Contains("Completed") &&
                                    h.Timestamp >= fromDate))
                .CountAsync();
        }
    }
}
