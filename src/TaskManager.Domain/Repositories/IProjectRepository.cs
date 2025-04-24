using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Repositories
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllByUserIdAsync(Guid userId);
        Task<Project> GetByIdWithTasksAsync(Guid id);
        Task<Project> AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<int> CountTasksAsync(Guid projectId);
    }
}
