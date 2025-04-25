using System.Reflection;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Tests.Builders
{
    public class ProjectBuilder
    {
        private string _name = "Test Project";
        private string _description = "Test Description";
        private Guid _userId = Guid.NewGuid();
        private readonly List<TaskItem> _tasks = new();
        private Guid? _customId = null;

        public ProjectBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ProjectBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public ProjectBuilder WithOwner(Guid userId)
        {
            _userId = userId;
            return this;
        }

        public ProjectBuilder WithTask(TaskItem task)
        {
            _tasks.Add(task);
            return this;
        }

        public ProjectBuilder WithId(Guid id)
        {
            _customId = id;
            return this;
        }

        public Project Build()
        {
            var project = new Project(_name, _description, _userId);

            if (_customId.HasValue)
            {
                typeof(BaseEntity)
                    .GetProperty("Id", BindingFlags.Public | BindingFlags.Instance)
                    ?.SetValue(project, _customId.Value, null);
            }

            foreach (var task in _tasks)
            {
                project.AddTask(task);
            }

            return project;
        }
    }
}
