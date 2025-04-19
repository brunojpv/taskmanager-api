using TaskManager.Application.DTOs.Activity;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _repository;
        private readonly IActivityHistoryService _activityHistoryService;

        public ActivityService(IActivityRepository repository, IActivityHistoryService activityHistoryService)
        {
            _repository = repository;
            _activityHistoryService = activityHistoryService;
        }

        public Task<IEnumerable<Activity>> GetAllAsync(Guid userId) => _repository.GetAllAsync(userId);

        public Task<Activity?> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

        public Task AddAsync(Activity task) => _repository.AddAsync(task);

        public Task UpdateAsync(Activity task) => _repository.UpdateAsync(task);

        public Task DeleteAsync(Guid id) => _repository.DeleteAsync(id);

        public async Task<Activity> CreateActivityAsync(CreateActivityDto dto)
        {
            var activity = new Activity(dto.Title, dto.Description, dto.DueDate, dto.Priority, dto.ProjectId);

            await _repository.AddAsync(activity);

            return activity;
        }

        public async Task UpdateActivityAsync(Guid activityId, UpdateActivityDto dto, Guid userId)
        {
            var activity = await _repository.GetByIdAsync(activityId);
            if (activity == null)
                throw new NotFoundException("Activity not found.");

            var changes = new List<string>();

            if (activity.Title != dto.Title)
                changes.Add($"Title changed from '{activity.Title}' to '{dto.Title}'.");

            if (activity.Description != dto.Description)
                changes.Add($"Description changed.");

            if (activity.DueDate != dto.DueDate)
                changes.Add($"DueDate changed from '{activity.DueDate}' to '{dto.DueDate}'.");

            if (activity.Status != dto.Status)
                changes.Add($"Status changed from '{activity.Status}' to '{dto.Status}'.");

            activity.UpdateDetails(dto.Title, dto.Description, dto.DueDate, dto.Status, dto.ProjectId);
            await _repository.UpdateAsync(activity);

            if (changes.Any())
            {
                string description = string.Join(" ", changes);
                await _activityHistoryService.RecordHistoryAsync(activity.Id, userId, description);
            }
        }
    }
}
