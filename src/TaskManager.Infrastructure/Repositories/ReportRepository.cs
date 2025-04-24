using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly TaskManagerDbContext _dbContext;

        public ReportRepository(TaskManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserTaskReport> GetUserTaskReportAsync(Guid userId, int days = 30)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return null;

            var fromDate = DateTime.UtcNow.AddDays(-days);
            var today = DateTime.UtcNow.Date;
            var nextWeekEnd = today.AddDays(7);
            var thisWeekEnd = today.AddDays(Math.Min(7, (int)DayOfWeek.Saturday - (int)today.DayOfWeek + 1));

            var userProjects = await _dbContext.Projects
                .Where(p => p.UserId == userId)
                .Include(p => p.Tasks)
                    .ThenInclude(t => t.History)
                .ToListAsync();

            var allTasks = userProjects.SelectMany(p => p.Tasks).ToList();

            int totalTasks = allTasks.Count;
            int completedTasks = allTasks.Count(t => t.Status == TaskItemStatus.Completed);
            int inProgressTasks = allTasks.Count(t => t.Status == TaskItemStatus.InProgress);
            int pendingTasks = allTasks.Count(t => t.Status == TaskItemStatus.Pending);

            int highPriorityTasks = allTasks.Count(t => t.Priority == TaskPriority.High);
            int mediumPriorityTasks = allTasks.Count(t => t.Priority == TaskPriority.Medium);
            int lowPriorityTasks = allTasks.Count(t => t.Priority == TaskPriority.Low);

            int overdueTasks = allTasks.Count(t => t.DueDate.HasValue && t.DueDate.Value.Date < today && t.Status != TaskItemStatus.Completed);
            int tasksDueToday = allTasks.Count(t => t.DueDate.HasValue && t.DueDate.Value.Date == today && t.Status != TaskItemStatus.Completed);
            int tasksDueThisWeek = allTasks.Count(t => t.DueDate.HasValue && t.DueDate.Value.Date > today && t.DueDate.Value.Date <= thisWeekEnd && t.Status != TaskItemStatus.Completed);
            int tasksDueNextWeek = allTasks.Count(t => t.DueDate.HasValue && t.DueDate.Value.Date > thisWeekEnd && t.DueDate.Value.Date <= nextWeekEnd && t.Status != TaskItemStatus.Completed);

            int completedTasksLastDays = allTasks.Count(t =>
                t.Status == TaskItemStatus.Completed &&
                t.History.Any(h =>
                    h.Action == "Status alterado" &&
                    h.Details != null &&
                    h.Details.Contains("Completed") &&
                    h.Timestamp >= fromDate));

            double completionRate = totalTasks > 0
                ? Math.Round((double)completedTasks / totalTasks * 100, 2)
                : 0;

            double averageCompletionTimeInDays = 0;
            var completeTasks = allTasks.Where(t => t.Status == TaskItemStatus.Completed).ToList();
            if (completeTasks.Any())
            {
                var completionTimes = new List<double>();

                foreach (var task in completeTasks)
                {
                    var creationEntry = task.History.FirstOrDefault(h => h.Action == "Tarefa criada");
                    var completionEntry = task.History.FirstOrDefault(h =>
                        h.Action == "Status alterado" &&
                        h.Details != null &&
                        h.Details.Contains("Completed"));

                    if (creationEntry != null && completionEntry != null)
                    {
                        var timeToComplete = (completionEntry.Timestamp - creationEntry.Timestamp).TotalDays;
                        completionTimes.Add(timeToComplete);
                    }
                }

                averageCompletionTimeInDays = completionTimes.Any()
                    ? Math.Round(completionTimes.Average(), 2)
                    : 0;
            }

            return new UserTaskReport(
                userId,
                user.Name,
                user.Email,
                user.IsManager,
                totalTasks,
                completedTasks,
                inProgressTasks,
                pendingTasks,
                highPriorityTasks,
                mediumPriorityTasks,
                lowPriorityTasks,
                overdueTasks,
                tasksDueToday,
                tasksDueThisWeek,
                tasksDueNextWeek,
                completionRate,
                averageCompletionTimeInDays,
                completedTasksLastDays,
                days
            );
        }

        public async Task<List<UserTaskReport>> GetAllUsersTaskReportAsync(int days = 30)
        {
            var users = await _dbContext.Users.ToListAsync();
            var reports = new List<UserTaskReport>();

            foreach (var user in users)
            {
                var report = await GetUserTaskReportAsync(user.Id, days);
                if (report != null)
                {
                    reports.Add(report);
                }
            }

            return reports;
        }

        public async Task<int> GetTotalCompletedTasksAsync(int days = 30)
        {
            var fromDate = DateTime.UtcNow.AddDays(-days);

            return await _dbContext.Tasks
                .CountAsync(t =>
                    t.Status == TaskItemStatus.Completed &&
                    t.History.Any(h =>
                        h.Action == "Status alterado" &&
                        h.Details != null &&
                        h.Details.Contains("Completed") &&
                        h.Timestamp >= fromDate));
        }

        public async Task<int> GetTotalUsersAsync()
        {
            return await _dbContext.Users.CountAsync();
        }

        public async Task<double> GetAverageCompletedTasksPerUserAsync(int days = 30)
        {
            int totalUsers = await GetTotalUsersAsync();
            if (totalUsers == 0)
                return 0;

            int totalCompletedTasks = await GetTotalCompletedTasksAsync(days);

            return Math.Round((double)totalCompletedTasks / totalUsers, 2);
        }
    }
}
