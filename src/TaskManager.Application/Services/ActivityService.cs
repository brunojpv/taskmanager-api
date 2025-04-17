using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services
{
    public class ActivityService : ITaskService
    {
        private readonly IActivityRepository _repository;

        public ActivityService(IActivityRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Activity>> GetAllAsync(Guid userId) => _repository.GetAllAsync(userId);

        public Task<Activity?> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

        public Task AddAsync(Activity task) => _repository.AddAsync(task);

        public Task UpdateAsync(Activity task) => _repository.UpdateAsync(task);

        public Task DeleteAsync(Guid id) => _repository.DeleteAsync(id);
    }
}
