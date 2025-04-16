using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<TaskItem>> GetAllAsync(Guid userId) => _repository.GetAllAsync(userId);

        public Task<TaskItem?> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

        public Task AddAsync(TaskItem task) => _repository.AddAsync(task);

        public Task UpdateAsync(TaskItem task) => _repository.UpdateAsync(task);

        public Task DeleteAsync(Guid id) => _repository.DeleteAsync(id);
    }
}
