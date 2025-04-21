using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Infrastructure.Data.Builders
{
    public class ActivityBuilder
    {
        private string _title = "Atividade";
        private string _description = "Descrição";
        private DateTime _dueDate = DateTime.UtcNow.AddDays(1);
        private ActivityPriority _priority = ActivityPriority.Medium;
        private ActivityStatus _status = ActivityStatus.Pending;
        private Guid? _projectId;
        private Project? _project;

        public ActivityBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public ActivityBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public ActivityBuilder WithDueDate(DateTime dueDate)
        {
            _dueDate = dueDate;
            return this;
        }

        public ActivityBuilder WithPriority(ActivityPriority priority)
        {
            _priority = priority;
            return this;
        }

        public ActivityBuilder WithStatus(ActivityStatus status)
        {
            _status = status;
            return this;
        }

        public ActivityBuilder WithProject(Project project)
        {
            _project = project;
            _projectId = project.Id;
            return this;
        }

        public ActivityBuilder WithProjectId(Guid projectId)
        {
            _projectId = projectId;
            return this;
        }

        public Activity Build()
        {
            if (_projectId is null)
                throw new InvalidOperationException("O ID do projeto é obrigatório para criar uma atividade.");

            var activity = new Activity(
                title: _title,
                description: _description,
                dueDate: _dueDate,
                priority: _priority,
                projectId: _projectId.Value
            )
            {
                Status = _status,
                Project = _project
            };

            return activity;
        }
    }
}
