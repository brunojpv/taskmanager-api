using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Repositories
{
    public interface ITaskCommentRepository
    {
        Task<List<TaskComment>> GetAllByTaskIdAsync(Guid taskId);
        Task<TaskComment> AddAsync(TaskComment comment);
    }
}
