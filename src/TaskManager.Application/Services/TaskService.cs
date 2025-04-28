using AutoMapper;
using System.Data;
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
        private readonly IMapper _mapper;

        public TaskService(
            ITaskRepository taskRepository,
            IProjectRepository projectRepository,
            ITaskHistoryRepository historyRepository,
            ITaskCommentRepository commentRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _historyRepository = historyRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _mapper = mapper;
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

            var changes = task.UpdateDetails(taskDto.Title, taskDto.Description, taskDto.DueDate, taskDto.Status, taskDto.UserId);

            await _taskRepository.UpdateAsync(task);

            if (changes.Count > 0)
            {
                await AddHistoryAsync(new TaskHistoryDTO
                {
                    Action = "Tarefa atualizada",
                    TaskId = task.Id,
                    Details = string.Join(", ", changes),
                    UserId = taskDto.UserId
                });
            }

            return _mapper.Map<TaskDTO>(task);
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

            return await _taskRepository.DeleteAsync(task);
        }

        public async Task<TaskDTO> AddCommentAsync(TaskCommentCreateDTO commentDto)
        {
            var task = await _taskRepository.GetByIdAsync(commentDto.TaskId);
            if (task == null)
            {
                throw new DomainException("Tarefa não encontrada.");
            }

            var taskComment = _mapper.Map<TaskComment>(commentDto);
            await _commentRepository.AddAsync(taskComment);

            await AddHistoryAsync(new TaskHistoryDTO
            {
                Action = "Comentário adicionado",
                TaskId = task.Id,
                Details = $"Comentário: {commentDto.Content}",
                UserId = commentDto.UserId
            });

            return await GetByIdAsync(task.Id);
        }

        public async Task AddHistoryAsync(TaskHistoryDTO taskHistoryDTO)
        {
            var taskHistoryEntry = _mapper.Map<TaskHistoryEntry>(taskHistoryDTO);
            await _historyRepository.AddAsync(taskHistoryEntry);
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
                    TaskId = entry.TaskId,
                    UserId = entry.UserId,
                    UserName = userName
                });
            }

            return historyDtos.OrderByDescending(h => h.TaskId).ToList();
        }
    }
}
