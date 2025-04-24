namespace TaskManager.Domain.Entities
{
    /// <summary>
    /// Representa um usuário do sistema.
    /// </summary>
    public class User : BaseEntity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public bool IsManager { get; private set; }
        public List<Project> Projects { get; private set; } = new();

        public User(string name, string email, bool isManager = false)
        {
            Name = name;
            Email = email;
            IsManager = isManager;
        }

        public void Update(string name, string email)
        {
            Name = name;
            Email = email;
            SetUpdated();
        }

        private User() { }
    }
}
