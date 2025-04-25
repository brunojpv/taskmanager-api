using System.Reflection;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Tests.Builders
{
    public class UserBuilder
    {
        private string _name = "Test User";
        private string _email = "test@example.com";
        private bool _isManager = false;
        private readonly List<Project> _projects = new();
        private Guid? _customId = null;

        public UserBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public UserBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public UserBuilder AsManager(bool isManager = true)
        {
            _isManager = isManager;
            return this;
        }

        public UserBuilder WithProject(string name, string description)
        {
            var projectId = Guid.NewGuid();
            var project = new Project(name, description, _customId ?? Guid.NewGuid());

            _projects.Add(project);
            return this;
        }

        public UserBuilder WithId(Guid id)
        {
            _customId = id;
            return this;
        }

        public User Build()
        {
            var user = new User(_name, _email, _isManager);

            if (_customId.HasValue)
            {
                typeof(BaseEntity)
                    .GetProperty("Id", BindingFlags.Public | BindingFlags.Instance)
                    ?.SetValue(user, _customId.Value, null);
            }

            return user;
        }
    }
}
