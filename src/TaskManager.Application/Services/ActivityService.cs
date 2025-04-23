using TaskManager.Application.DTOs.Activity;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _repository;
        private readonly IActivityHistoryService _activityHistoryService;
        private readonly IProjectService _projectService;

        public ActivityService(
            IActivityRepository repository,
            IActivityHistoryService activityHistoryService,
            IProjectService projectService)
        {
            _repository = repository;
            _activityHistoryService = activityHistoryService;
            _projectService = projectService;
        }

        public Task<IEnumerable<Activity>> GetAllAsync(Guid? userId) => _repository.GetAllAsync(userId);
        public Task<Activity?> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);
        public Task AddAsync(Activity activity) => _repository.AddAsync(activity);
        public Task UpdateAsync(Activity activity) => _repository.UpdateAsync(activity);
        public Task DeleteAsync(Guid id) => _repository.DeleteAsync(id);

        public async Task<IEnumerable<ActivityDto>> GetAllActivityAsync(Guid? userId)
        {
            var activities = await _repository.GetAllAsync(userId);

            return activities.Select(MapToDto);
        }

        public async Task<ActivityDto> CreateActivityAsync(CreateActivityDto dto)
        {
            var project = await _projectService.GetByIdAsync(dto.ProjectId)
                          ?? throw new NotFoundException("Projeto não encontrado.");

            if (!project.CanAddNewActivity())
                throw new BusinessException("O projeto já possui o número máximo de 20 tarefas.");

            var activity = Activity.Create(dto.Title, dto.Description, dto.DueDate, dto.Priority, dto.ProjectId);
            await _repository.AddAsync(activity);

            return MapToDto(activity);
        }

        public async Task<ActivityDto?> UpdateActivityAsync(Guid activityId, UpdateActivityDto dto, Guid? userId)
        {
            var activity = await _repository.GetByIdAsync(activityId)
                          ?? throw new NotFoundException("Tarefa não encontrada.");

            var changes = new List<string>();

            if (activity.Title != dto.Title)
                changes.Add($"Título alterado de '{activity.Title}' para '{dto.Title}'.");

            if (activity.Description != dto.Description)
                changes.Add("Descrição alterada.");

            if (activity.DueDate != dto.DueDate)
                changes.Add($"Data de vencimento alterada de '{activity.DueDate}' para '{dto.DueDate}'.");

            if (activity.Status != dto.Status)
                changes.Add($"Status alterado de '{activity.Status}' para '{dto.Status}'.");

            activity.Update(dto.Title, dto.Description, dto.DueDate, dto.Status, dto.Priority);
            await _repository.UpdateAsync(activity);

            if (changes.Any())
            {
                var description = string.Join(" ", changes);
                await _activityHistoryService.RecordHistoryAsync(activity.Id, userId, description);
            }

            return MapToDto(activity);
        }

        public async Task<bool> DeleteActivityAsync(Guid activityId, Guid? userId)
        {
            var activity = await _repository.GetByIdAsync(activityId);
            if (activity is null || activity.Project?.UserId != userId)
                return false;

            await _repository.DeleteAsync(activityId);
            return true;
        }

        /// <summary>
        /// Mapeia uma entidade Activity para ActivityDto.
        /// </summary>
        private static ActivityDto MapToDto(Activity activity) => new()
        {
            Id = activity.Id,
            Title = activity.Title,
            Description = activity.Description,
            DueDate = activity.DueDate,
            Status = activity.Status,
            Priority = activity.Priority,
            CreatedAt = activity.CreatedAt,
            ProjectId = activity.ProjectId
        };
    }
}
