using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Data.Builders
{
    public class ProjectBuilder
    {
        private string _name = "Projeto Padrão";
        private string _description = "Descrição do projeto";
        private User? _user;
        private Guid? _userId;

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
            _userId = user.Id;
            return this;
        }

        public ProjectBuilder WithUserId(Guid userId)
        {
            _userId = userId;
            return this;
        }

        public Project Build()
        {
            if (_userId is null)
                throw new InvalidOperationException("O ID do usuário é obrigatório para criar um projeto.");

            var project = new Project(_name, _description, _userId.Value)
            {
                User = _user
            };

            return project;
        }
    }
}
