using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Repositories;

namespace TaskManager.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskHistoryRepository _historyRepository;
        private readonly ITaskCommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;

        public TaskService(
            ITaskRepository taskRepository,
            IProjectRepository projectRepository,
            ITaskHistoryRepository historyRepository,
            ITaskCommentRepository commentRepository,
            IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _historyRepository = historyRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        public async Task<List<TaskDTO>> GetAllByProjectIdAsync(Guid projectId)
        {
            if (!await _projectRepository.ExistsAsync(projectId))
            {
                throw new DomainException("Projeto não encontrado.");
            }

            var tasks = await _taskRepository.GetAllByProjectIdAsync(projectId);
            var taskDtos = new List<TaskDTO>();

            foreach (var task in tasks)
            {
                var comments = await _commentRepository.GetAllByTaskIdAsync(task.Id);
                var commentDtos = new List<TaskCommentDTO>();

                foreach (var comment in comments)
                {
                    var user = await _userRepository.GetByIdAsync(comment.UserId);

                    commentDtos.Add(new TaskCommentDTO
                    {
                        Id = comment.Id,
                        Content = comment.Content,
                        CreatedAt = comment.CreatedAt,
                        UserId = comment.UserId,
                        UserName = user?.Name ?? "Usuário Desconhecido"
                    });
                }

                taskDtos.Add(new TaskDTO
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    CreatedAt = task.CreatedAt,
                    UpdatedAt = task.UpdatedAt,
                    DueDate = task.DueDate,
                    Status = task.Status,
                    Priority = task.Priority,
                    ProjectId = task.ProjectId,
                    Comments = commentDtos
                });
            }

            return taskDtos;
        }

        public async Task<TaskDTO> GetByIdAsync(Guid id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                throw new DomainException("Tarefa não encontrada.");
            }

            var comments = await _commentRepository.GetAllByTaskIdAsync(task.Id);
            var commentDtos = new List<TaskCommentDTO>();

            foreach (var comment in comments)
            {
                var user = await _userRepository.GetByIdAsync(comment.UserId);

                commentDtos.Add(new TaskCommentDTO
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    CreatedAt = comment.CreatedAt,
                    UserId = comment.UserId,
                    UserName = user?.Name ?? "Usuário Desconhecido"
                });
            }

            return new TaskDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                DueDate = task.DueDate,
                Status = task.Status,
                Priority = task.Priority,
                ProjectId = task.ProjectId,
                Comments = commentDtos
            };
        }

        public async Task<TaskDTO> CreateAsync(TaskCreateDTO taskDto)
        {
            var project = await _projectRepository.GetByIdWithTasksAsync(taskDto.ProjectId);
            if (project == null)
            {
                throw new DomainException("Projeto não encontrado.");
            }

            if (project.Tasks.Count >= 20)
            {
                throw new DomainException("Limite máximo de 20 tarefas por projeto atingido.");
            }

            var task = new TaskItem(
                taskDto.Title,
                taskDto.Description,
                taskDto.DueDate,
                taskDto.Priority,
                taskDto.ProjectId
            );

            project.AddTask(task);
            await _taskRepository.AddAsync(task);

            return new TaskDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                DueDate = task.DueDate,
                Status = task.Status,
                Priority = task.Priority,
                ProjectId = task.ProjectId,
                Comments = new List<TaskCommentDTO>()
            };
        }

        public async Task<TaskDTO> UpdateAsync(TaskUpdateDTO taskDto)
        {
            var task = await _taskRepository.GetByIdAsync(taskDto.Id);
            if (task == null)
            {
                throw new DomainException("Tarefa não encontrada.");
            }

            task.UpdateDetails(taskDto.Title, taskDto.Description, taskDto.DueDate, taskDto.UserId);
            task.UpdateStatus(taskDto.Status, taskDto.UserId);

            await _taskRepository.UpdateAsync(task);

            var comments = await _commentRepository.GetAllByTaskIdAsync(task.Id);
            var commentDtos = new List<TaskCommentDTO>();

            foreach (var comment in comments)
            {
                var user = await _userRepository.GetByIdAsync(comment.UserId);

                commentDtos.Add(new TaskCommentDTO
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    CreatedAt = comment.CreatedAt,
                    UserId = comment.UserId,
                    UserName = user?.Name ?? "Usuário Desconhecido"
                });
            }

            return new TaskDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                DueDate = task.DueDate,
                Status = task.Status,
                Priority = task.Priority,
                ProjectId = task.ProjectId,
                Comments = commentDtos
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                throw new DomainException("Tarefa não encontrada.");
            }

            var project = await _projectRepository.GetByIdWithTasksAsync(task.ProjectId);
            if (project == null)
            {
                throw new DomainException("Projeto não encontrado.");
            }

            project.RemoveTask(task.Id);
            await _projectRepository.UpdateAsync(project);

            return await _taskRepository.DeleteAsync(id);
        }

        public async Task<TaskDTO> AddCommentAsync(TaskCommentCreateDTO commentDto)
        {
            var task = await _taskRepository.GetByIdAsync(commentDto.TaskId);
            if (task == null)
            {
                throw new DomainException("Tarefa não encontrada.");
            }

            task.AddComment(commentDto.Content, commentDto.UserId);
            await _taskRepository.UpdateAsync(task);

            return await GetByIdAsync(task.Id);
        }

        public async Task<List<TaskHistoryDTO>> GetHistoryAsync(Guid taskId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
            {
                throw new DomainException("Tarefa não encontrada.");
            }

            var history = await _historyRepository.GetAllByTaskIdAsync(taskId);
            var historyDtos = new List<TaskHistoryDTO>();

            foreach (var entry in history)
            {
                string userName = "Sistema";

                if (entry.UserId.HasValue)
                {
                    var user = await _userRepository.GetByIdAsync(entry.UserId.Value);
                    userName = user?.Name ?? "Usuário Desconhecido";
                }

                historyDtos.Add(new TaskHistoryDTO
                {
                    Id = entry.Id,
                    Action = entry.Action,
                    Details = entry.Details,
                    Timestamp = entry.Timestamp,
                    UserId = entry.UserId,
                    UserName = userName
                });
            }

            return historyDtos.OrderByDescending(h => h.Timestamp).ToList();
        }
    }
}
