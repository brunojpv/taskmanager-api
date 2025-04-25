using TaskManager.Domain.Entities;
using TaskManager.Domain.Repositories;

namespace TaskManager.Application.Tests.Mocks
{
    public class RepositoryMockFactory : IProjectRepository
    {
        private readonly List<Project> _projects = new List<Project>();

        public Task<List<Project>> GetAllByUserIdAsync(Guid userId)
        {
            return Task.FromResult(_projects.Where(p => p.UserId == userId).ToList());
        }

        public Task<Project> GetByIdWithTasksAsync(Guid id)
        {
            return Task.FromResult(_projects.FirstOrDefault(p => p.Id == id));
        }

        public Task<Project> AddAsync(Project project)
        {
            _projects.Add(project);
            return Task.FromResult(project);
        }

        public Task UpdateAsync(Project project)
        {
            var index = _projects.FindIndex(p => p.Id == project.Id);
            if (index >= 0)
            {
                _projects[index] = project;
            }
            return Task.CompletedTask;
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            var project = _projects.FirstOrDefault(p => p.Id == id);
            if (project != null)
            {
                _projects.Remove(project);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return Task.FromResult(_projects.Any(p => p.Id == id));
        }

        public Task<int> CountTasksAsync(Guid projectId)
        {
            var project = _projects.FirstOrDefault(p => p.Id == projectId);
            if (project != null)
            {
                return Task.FromResult(project.Tasks.Count);
            }
            return Task.FromResult(0);
        }
    }
}
