using Microsoft.EntityFrameworkCore;
using TaskManager.Application.DTOs.User;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserTaskReportDto>> GetUserPerformanceReportAsync(DateTime cutoff)
        {
            var query = from u in _context.Users
                        let completedTasks = u.Projects
                            .SelectMany(p => p.Activities)
                            .Where(a => a.Status == ActivityStatus.Completed && a.DueDate >= cutoff)
                        select new UserTaskReportDto
                        {
                            UserName = u.Name,
                            UserEmail = u.Email,
                            TotalCompletedTasks = completedTasks.Count(),
                            AverageCompletedTasksPerDay = completedTasks.Count() / 30.0
                        };

            return await query.ToListAsync();
        }
    }
}
