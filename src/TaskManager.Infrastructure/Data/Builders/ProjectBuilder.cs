using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Data.Builders
{
    public class ProjectBuilder
    {
        private string _name = "Projeto Padrão";
        private string _description = "Descrição do projeto";
        private User? _user;

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

        public ProjectBuilder WithUser(User user)
        {
            _user = user;
            return this;
        }

        public Project Build()
        {
            return new Project
            {
                Name = _name,
                Description = _description,
                User = _user ?? throw new ArgumentNullException(nameof(_user))
            };
        }
    }
}
