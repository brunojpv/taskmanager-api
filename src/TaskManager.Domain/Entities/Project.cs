using TaskManager.Domain.Enums;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Domain.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public List<TaskItem> Tasks { get; private set; } = new();

        private const int MaxTasksPerProject = 20;

        public Project(string name, string description, Guid userId)
        {
            Name = name;
            Description = description;
            UserId = userId;
        }

        private Project() { }

        public void AddTask(TaskItem task)
        {
            if (Tasks.Count >= MaxTasksPerProject)
            {
                throw new DomainException("Limite máximo de 20 tarefas por projeto atingido.");
            }

            Tasks.Add(task);
        }

        public void RemoveTask(Guid taskId)
        {
            var task = Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                throw new DomainException("Tarefa não encontrada.");
            }

            Tasks.Remove(task);
        }

        public bool HasPendingTasks()
        {
            return Tasks.Any(t => t.Status != TaskItemStatus.Completed);
        }

        public void Update(string name, string description)
        {
            Name = name;
            Description = description;
            SetUpdated();
        }
    }
}
