namespace TaskManager.Application.DTOs
{
    public class PerformanceReportDTO
    {
        public double AverageCompletedTasksPerUser { get; set; }
        public int TotalCompletedTasks { get; set; }
        public int TotalUsers { get; set; }
        public DateTime ReportDate { get; set; } = DateTime.UtcNow;
        public int DaysInReport { get; set; }
    }
}
