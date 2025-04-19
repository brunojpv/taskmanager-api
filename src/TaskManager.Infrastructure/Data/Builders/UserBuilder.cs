using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Data.Builders
{
    public class UserBuilder
    {
        private string _name = "Default User";
        private string _email = "user@default.com";
        private string _password = "123456";

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

        public UserBuilder WithPassword(string password)
        {
            _password = password;
            return this;
        }

        public User Build()
        {
            return new User
            {
                Name = _name,
                Email = _email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(_password)
            };
        }
    }
}
