﻿using TaskManager.Domain.Entities;
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
            return this;
        }

        public Activity Build()
        {
            var activity = new Activity(
                title: _title,
                description: _description,
                dueDate: _dueDate,
                priority: _priority,
                projectId: _project?.Id ?? throw new ArgumentNullException(nameof(_project))
            )
            {
                Status = _status,
                Project = _project
            };

            return activity;
        }
    }
}
