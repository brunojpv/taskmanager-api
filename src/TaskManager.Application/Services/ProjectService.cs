using TaskManager.Application.DTOs.Project;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
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

        public async Task<Project> CreateActivityAsync(CreateProjectDto dto, Guid userId)
        {
            var project = new Project(dto.Name, dto.Description, userId);

            await _repository.AddAsync(project);

            return project;
        }

        public async Task UpdateProjectAsync(Guid projectId, UpdateProjectDto dto, Guid userId)
        {
            var project = await _repository.GetByIdAsync(projectId) ?? throw new NotFoundException("Project not found.");

            var changes = new List<string>();

            if (project.Name != dto.Name)
                changes.Add($"Title changed from '{project.Name}' to '{dto.Name}'.");

            if (project.Description != dto.Description)
                changes.Add($"Description changed.");

            project.UpdateDetails(dto.Name, dto.Description, userId);
            await _repository.UpdateAsync(project);
        }

        public async Task DeleteProjectAsync(Guid projectId, Guid userId)
        {
            var project = await _repository.GetByIdAsync(projectId) ?? throw new NotFoundException("Project not found.");

            if (project.UserId != userId)
                throw new NotFoundException("User not found.");

            bool hasPendingActivities = project.Activities.Any(a => a.Status != ActivityStatus.Completed);
            if (hasPendingActivities)
                throw new InvalidOperationException("Cannot delete project with pending activities. Complete or delete these activities first.");

            await _repository.DeleteAsync(projectId);
        }
    }
}
