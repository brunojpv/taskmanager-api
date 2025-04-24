using TaskManager.Application.DTOs;

namespace TaskManager.Application.Services
{
    public interface IReportService
    {
        Task<UserTaskReportDto> GetUserTaskReportAsync(Guid userId, int days = 30);
        Task<List<UserTaskReportDto>> GetAllUsersTaskReportAsync(int days = 30);
        Task<PerformanceReportDTO> GetTeamPerformanceReportAsync(int days = 30);
    }
}
