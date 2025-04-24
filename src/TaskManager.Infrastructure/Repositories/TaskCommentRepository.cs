using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class TaskCommentRepository : ITaskCommentRepository
    {
        private readonly TaskManagerDbContext _dbContext;

        public TaskCommentRepository(TaskManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TaskComment>> GetAllByTaskIdAsync(Guid taskId)
        {
            return await _dbContext.TaskComments
                .Where(c => c.TaskId == taskId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<TaskComment> AddAsync(TaskComment comment)
        {
            await _dbContext.TaskComments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();
            return comment;
        }
    }
}
