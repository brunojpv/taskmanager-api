using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Repositories;

namespace TaskManager.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<List<ProjectDTO>> GetAllByUserIdAsync(Guid userId)
        {
            var projects = await _projectRepository.GetAllByUserIdAsync(userId);

            var projectDtos = new List<ProjectDTO>();
            foreach (var project in projects)
            {
                var taskCount = await _projectRepository.CountTasksAsync(project.Id);

                projectDtos.Add(new ProjectDTO
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    CreatedAt = project.CreatedAt,
                    TaskCount = taskCount
                });
            }

            return projectDtos;
        }

        public async Task<ProjectDTO> GetByIdAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdWithTasksAsync(id);
            if (project == null)
            {
                throw new DomainException("Projeto não encontrado.");
            }

            return new ProjectDTO
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                TaskCount = project.Tasks.Count
            };
        }

        public async Task<ProjectDTO> CreateAsync(ProjectCreateDTO projectDto)
        {
            var project = new Project(
                projectDto.Name,
                projectDto.Description,
                projectDto.UserId
            );

            await _projectRepository.AddAsync(project);

            return new ProjectDTO
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                TaskCount = 0
            };
        }

        public async Task<ProjectDTO> UpdateAsync(ProjectUpdateDTO projectDto)
        {
            var project = await _projectRepository.GetByIdWithTasksAsync(projectDto.Id);
            if (project == null)
            {
                throw new DomainException("Projeto não encontrado.");
            }

            project.Update(projectDto.Name, projectDto.Description);
            await _projectRepository.UpdateAsync(project);

            return new ProjectDTO
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                TaskCount = project.Tasks.Count
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdWithTasksAsync(id);
            if (project == null)
            {
                throw new DomainException("Projeto não encontrado.");
            }

            if (project.HasPendingTasks())
            {
                throw new DomainException("Não é possível remover um projeto com tarefas pendentes. Complete ou remova as tarefas primeiro.");
            }

            return await _projectRepository.DeleteAsync(id);
        }
    }
}
