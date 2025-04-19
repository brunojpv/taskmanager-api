namespace TaskManager.Application.DTOs.User
{
    public class UserTaskReportDto
    {
        public string UserName { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public int TotalCompletedTasks { get; set; }
        public double AverageCompletedTasksPerDay { get; set; }
    }
}
