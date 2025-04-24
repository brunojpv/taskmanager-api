using TaskManager.Domain.Entities;

namespace TaskManager.Application.DTOs
{
    public class UserTaskReportDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsManager { get; set; }

        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int PendingTasks { get; set; }

        public int HighPriorityTasks { get; set; }
        public int MediumPriorityTasks { get; set; }
        public int LowPriorityTasks { get; set; }

        public int OverdueTasks { get; set; }
        public int TasksDueToday { get; set; }
        public int TasksDueThisWeek { get; set; }
        public int TasksDueNextWeek { get; set; }

        public double CompletionRate { get; set; }
        public double AverageCompletionTimeInDays { get; set; }
        public int CompletedTasksLast30Days { get; set; }

        public DateTime ReportGeneratedAt { get; set; }
        public int DaysInReport { get; set; }

        public double TasksCompletedPerDay { get; set; }
        public double EfficiencyScore { get; set; }

        public static UserTaskReportDto FromEntity(UserTaskReport entity)
        {
            return new UserTaskReportDto
            {
                UserId = entity.UserId,
                UserName = entity.UserName,
                Email = entity.Email,
                IsManager = entity.IsManager,
                TotalTasks = entity.TotalTasks,
                CompletedTasks = entity.CompletedTasks,
                InProgressTasks = entity.InProgressTasks,
                PendingTasks = entity.PendingTasks,
                HighPriorityTasks = entity.HighPriorityTasks,
                MediumPriorityTasks = entity.MediumPriorityTasks,
                LowPriorityTasks = entity.LowPriorityTasks,
                OverdueTasks = entity.OverdueTasks,
                TasksDueToday = entity.TasksDueToday,
                TasksDueThisWeek = entity.TasksDueThisWeek,
                TasksDueNextWeek = entity.TasksDueNextWeek,
                CompletionRate = entity.CompletionRate,
                AverageCompletionTimeInDays = entity.AverageCompletionTimeInDays,
                CompletedTasksLast30Days = entity.CompletedTasksLastDays,
                ReportGeneratedAt = entity.ReportGeneratedAt,
                DaysInReport = entity.DaysInReport,
                TasksCompletedPerDay = entity.TasksCompletedPerDay,
                EfficiencyScore = entity.EfficiencyScore
            };
        }
    }
}
