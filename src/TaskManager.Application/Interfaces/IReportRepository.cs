using TaskManager.Application.DTOs.User;

namespace TaskManager.Domain.Interfaces
{
    public interface IReportRepository
    {
        Task<List<UserTaskReportDto>> GetUserPerformanceReportAsync(DateTime cutoff);
    }
}
