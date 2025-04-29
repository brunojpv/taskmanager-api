using TaskManager.Application.DTOs;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Repositories;

namespace TaskManager.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IUserRepository _userRepository;

        public ReportService(IReportRepository reportRepository, IUserRepository userRepository)
        {
            _reportRepository = reportRepository;
            _userRepository = userRepository;
        }

        public async Task<UserTaskReportDto> GetUserTaskReportAsync(Guid userId, int days = 30)
        {
            var userExists = await _userRepository.GetByIdAsync(userId) != null;
            if (!userExists)
            {
                throw new DomainException("Usuário não encontrado.");
            }

            var report = await _reportRepository.GetUserTaskReportAsync(userId, days);
            if (report == null)
            {
                throw new DomainException("Não foi possível gerar o relatório de tarefas do usuário.");
            }

            return UserTaskReportDto.FromEntity(report);
        }

        public async Task<List<UserTaskReportDto>> GetAllUsersTaskReportAsync(int days = 30)
        {
            var reports = await _reportRepository.GetAllUsersTaskReportAsync(days);
            return reports.Select(UserTaskReportDto.FromEntity).ToList();
        }

        public async Task<PerformanceReportDTO> GetTeamPerformanceReportAsync(int days = 30)
        {
            int totalCompletedTasks = await _reportRepository.GetTotalCompletedTasksAsync(days);
            int totalUsers = await _reportRepository.GetTotalUsersAsync();
            double averageTasksPerUser = await _reportRepository.GetAverageCompletedTasksPerUserAsync(days);

            return new PerformanceReportDTO
            {
                TotalCompletedTasks = totalCompletedTasks,
                TotalUsers = totalUsers,
                AverageCompletedTasksPerUser = averageTasksPerUser,
                DaysInReport = days,
                ReportDate = DateTime.UtcNow
            };
        }
    }
}
