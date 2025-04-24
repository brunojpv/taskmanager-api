using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Repositories
{
    public interface IReportRepository
    {
        Task<UserTaskReport> GetUserTaskReportAsync(Guid userId, int days = 30);
        Task<List<UserTaskReport>> GetAllUsersTaskReportAsync(int days = 30);
        Task<int> GetTotalCompletedTasksAsync(int days = 30);
        Task<int> GetTotalUsersAsync();
        Task<double> GetAverageCompletedTasksPerUserAsync(int days = 30);
    }
}
