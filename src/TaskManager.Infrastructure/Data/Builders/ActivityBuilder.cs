using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Infrastructure.Data.Builders
{
    /// <summary>
    /// Construtor fluente para instanciar objetos do tipo Activity com flexibilidade, ideal para testes e seeds.
    /// </summary>
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
            if (_projectId is null || _projectId == Guid.Empty)
                throw new InvalidOperationException("O ID do projeto é obrigatório para criar uma atividade.");

            var activity = Activity.Create(
                title: _title,
                description: _description,
                dueDate: _dueDate,
                priority: _priority,
                projectId: _projectId.Value
            );

            if (_project is not null)
                activity.SetProject(_project);

            if (activity.Status != _status)
                activity.SetStatus(_status);

            return activity;
        }
    }
}
