using TaskManager.Application.DTOs.User;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<List<UserTaskReportDto>> GetUserPerformanceReportAsync()
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-30);
            return await _reportRepository.GetUserPerformanceReportAsync(cutoffDate);
        }
    }
}
