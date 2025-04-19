using TaskManager.Application.DTOs.User;

namespace TaskManager.Application.Interfaces
{
    public interface IReportService
    {
        Task<List<UserTaskReportDto>> GetUserPerformanceReportAsync();
    }
}
