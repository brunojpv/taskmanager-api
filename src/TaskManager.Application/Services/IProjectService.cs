using TaskManager.Application.DTOs;

namespace TaskManager.Application.Services
{
    public interface IProjectService
    {
        Task<List<ProjectDTO>> GetAllByUserIdAsync(Guid userId);
        Task<ProjectDTO> GetByIdAsync(Guid id);
        Task<ProjectDTO> CreateAsync(ProjectCreateDTO projectDto);
        Task<ProjectDTO> UpdateAsync(ProjectUpdateDTO projectDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
