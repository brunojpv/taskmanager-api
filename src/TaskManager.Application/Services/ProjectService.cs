using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repository;

        public ProjectService(IProjectRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Project>> GetAllAsync(Guid userId) => _repository.GetAllAsync(userId);

        public Task<Project?> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

        public Task AddAsync(Project project) => _repository.AddAsync(project);

        public Task UpdateAsync(Project project) => _repository.UpdateAsync(project);

        public Task DeleteAsync(Guid id) => _repository.DeleteAsync(id);
    }
}
