using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities
{
    /// <summary>
    /// Representa um usuário do sistema.
    /// </summary>
    public class User : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public UserRole Role { get; private set; } = UserRole.Regular;

        private readonly List<Project> _projects = new();
        public IReadOnlyCollection<Project> Projects => _projects;

        protected User() { }

        public User(string name, string email, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome não pode ser vazio ou nulo.", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email não pode ser vazio ou nulo.", nameof(email));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Hash da senha não pode ser vazio ou nulo.", nameof(passwordHash));

            Name = name;
            Email = email;
            PasswordHash = passwordHash;
        }

        public static User Create(string name, string email, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome não pode ser vazio ou nulo.", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email não pode ser vazio ou nulo.", nameof(email));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Hash da senha não pode ser vazio ou nulo.", nameof(passwordHash));

            return new User(name, email, passwordHash);
        }

        public void AddProject(Project project)
        {
            _projects.Add(project);
        }
    }
}
