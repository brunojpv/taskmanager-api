using System.Reflection;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Tests.Builders
{
    public class TaskItemBuilder
    {
        private string _title = "Test Task";
        private string _description = "Test Description";
        private DateTime? _dueDate = DateTime.UtcNow.AddDays(7);
        private TaskPriority _priority = TaskPriority.Medium;
        private TaskItemStatus _status = TaskItemStatus.Pending;
        private Guid _projectId = Guid.NewGuid();
        private readonly List<TaskComment> _comments = new();
        private readonly List<TaskHistoryEntry> _historyEntries = new();
        private Guid? _customId = null;

        public TaskItemBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public TaskItemBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public TaskItemBuilder WithDueDate(DateTime? dueDate)
        {
            _dueDate = dueDate;
            return this;
        }

        public TaskItemBuilder WithPriority(TaskPriority priority)
        {
            _priority = priority;
            return this;
        }

        public TaskItemBuilder WithStatus(TaskItemStatus status)
        {
            _status = status;
            return this;
        }

        public TaskItemBuilder InProject(Guid projectId)
        {
            _projectId = projectId;
            return this;
        }

        public TaskItemBuilder InProject(Project project)
        {
            _projectId = project.Id;
            return this;
        }

        public TaskItemBuilder WithComment(string content, Guid userId)
        {
            _comments.Add(new TaskComment(content, _customId ?? Guid.NewGuid(), userId));
            return this;
        }

        public TaskItemBuilder WithHistoryEntry(string action, string details, Guid? userId = null)
        {
            _historyEntries.Add(new TaskHistoryEntry(action, _customId ?? Guid.NewGuid(), details, userId));
            return this;
        }

        public TaskItemBuilder WithId(Guid id)
        {
            _customId = id;
            return this;
        }

        public TaskItem Build()
        {
            var task = new TaskItem(_title, _description, _dueDate, _priority, _projectId);

            if (_customId.HasValue)
            {
                typeof(BaseEntity)
                    .GetProperty("Id", BindingFlags.Public | BindingFlags.Instance)
                    ?.SetValue(task, _customId.Value, null);
            }

            if (_status != TaskItemStatus.Pending)
            {
                task.UpdateStatus(_status, Guid.NewGuid());
            }

            foreach (var comment in _comments)
            {
                task.AddComment(comment.Content, comment.UserId);
            }

            return task;
        }
    }
}
