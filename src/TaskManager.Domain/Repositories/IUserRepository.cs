using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid id);
        Task<bool> IsManagerAsync(Guid userId);
    }
}
