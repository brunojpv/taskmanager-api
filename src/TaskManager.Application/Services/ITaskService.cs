using TaskManager.Application.DTOs;

namespace TaskManager.Application.Services
{
    public interface ITaskService
    {
        Task<List<TaskDTO>> GetAllByProjectIdAsync(Guid projectId);
        Task<TaskDTO> GetByIdAsync(Guid id);
        Task<TaskDTO> CreateAsync(TaskCreateDTO taskDto);
        Task<TaskDTO> UpdateAsync(TaskUpdateDTO taskDto);
        Task<bool> DeleteAsync(Guid id);
        Task<TaskDTO> AddCommentAsync(TaskCommentCreateDTO commentDto);
        Task<List<TaskHistoryDTO>> GetHistoryAsync(Guid taskId);
    }
}
