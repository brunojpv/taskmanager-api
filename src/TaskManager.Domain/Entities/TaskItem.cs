using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities
{
    public class TaskItem : BaseEntity
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime? DueDate { get; private set; }
        public TaskItemStatus Status { get; private set; }
        public TaskPriority Priority { get; private set; }
        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; }
        public List<TaskHistoryEntry> History { get; private set; } = new();
        public List<TaskComment> Comments { get; private set; } = new();

        public TaskItem(string title, string description, DateTime? dueDate, TaskPriority priority, Guid projectId)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            Status = TaskItemStatus.Pending;
            Priority = priority;
            ProjectId = projectId;

            AddHistoryEntry("Tarefa criada", null, null);
        }

        private TaskItem() { }

        public void UpdateStatus(TaskItemStatus newStatus, Guid userId)
        {
            if (Status == newStatus)
                return;

            var oldStatus = Status;
            Status = newStatus;
            SetUpdated();

            AddHistoryEntry("Status alterado",
                $"Status alterado de {oldStatus} para {newStatus}",
                userId);
        }

        public void UpdateDetails(string title, string description, DateTime? dueDate, Guid userId)
        {
            var changes = new List<string>();

            if (Title != title)
            {
                changes.Add($"Título alterado de '{Title}' para '{title}'");
                Title = title;
            }

            if (Description != description)
            {
                changes.Add($"Descrição atualizada");
                Description = description;
            }

            if (DueDate != dueDate)
            {
                changes.Add($"Data de vencimento alterada de '{DueDate}' para '{dueDate}'");
                DueDate = dueDate;
            }

            if (changes.Any())
            {
                SetUpdated();
                AddHistoryEntry("Detalhes atualizados", string.Join(", ", changes), userId);
            }
        }

        public void AddComment(string content, Guid userId)
        {
            var comment = new TaskComment(content, Id, userId);
            Comments.Add(comment);

            AddHistoryEntry("Comentário adicionado", $"Comentário: {content}", userId);
        }

        private void AddHistoryEntry(string action, string details, Guid? userId)
        {
            History.Add(new TaskHistoryEntry(action, details, Id, userId));
        }
    }
}
