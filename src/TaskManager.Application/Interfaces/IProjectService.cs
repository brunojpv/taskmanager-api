using TaskManager.Application.DTOs.Project;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllAsync(Guid userId);
        Task<Project?> GetByIdAsync(Guid id);
        Task AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Guid id);
        Task<Project> CreateProjectAsync(CreateProjectDto dto, Guid userId);
        Task UpdateProjectAsync(Guid projectId, UpdateProjectDto dto, Guid userId);
        Task DeleteProjectAsync(Guid projectId, Guid userId);
    }
}
