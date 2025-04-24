namespace TaskManager.Domain.Entities
{
    public class UserTaskReport
    {
        public Guid UserId { get; private set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }
        public bool IsManager { get; private set; }

        // Estatísticas de tarefas
        public int TotalTasks { get; private set; }
        public int CompletedTasks { get; private set; }
        public int InProgressTasks { get; private set; }
        public int PendingTasks { get; private set; }

        // Estatísticas por prioridade
        public int HighPriorityTasks { get; private set; }
        public int MediumPriorityTasks { get; private set; }
        public int LowPriorityTasks { get; private set; }

        // Estatísticas por prazo
        public int OverdueTasks { get; private set; }
        public int TasksDueToday { get; private set; }
        public int TasksDueThisWeek { get; private set; }
        public int TasksDueNextWeek { get; private set; }

        // Métricas de desempenho
        public double CompletionRate { get; private set; }
        public double AverageCompletionTimeInDays { get; private set; }
        public int CompletedTasksLastDays { get; private set; }

        // Informações do relatório
        public DateTime ReportGeneratedAt { get; private set; }
        public int DaysInReport { get; private set; }

        public UserTaskReport(
            Guid userId,
            string userName,
            string email,
            bool isManager,
            int totalTasks,
            int completedTasks,
            int inProgressTasks,
            int pendingTasks,
            int highPriorityTasks,
            int mediumPriorityTasks,
            int lowPriorityTasks,
            int overdueTasks,
            int tasksDueToday,
            int tasksDueThisWeek,
            int tasksDueNextWeek,
            double completionRate,
            double averageCompletionTimeInDays,
            int completedTasksLastDays,
            int daysInReport)
        {
            UserId = userId;
            UserName = userName;
            Email = email;
            IsManager = isManager;
            TotalTasks = totalTasks;
            CompletedTasks = completedTasks;
            InProgressTasks = inProgressTasks;
            PendingTasks = pendingTasks;
            HighPriorityTasks = highPriorityTasks;
            MediumPriorityTasks = mediumPriorityTasks;
            LowPriorityTasks = lowPriorityTasks;
            OverdueTasks = overdueTasks;
            TasksDueToday = tasksDueToday;
            TasksDueThisWeek = tasksDueThisWeek;
            TasksDueNextWeek = tasksDueNextWeek;
            CompletionRate = completionRate;
            AverageCompletionTimeInDays = averageCompletionTimeInDays;
            CompletedTasksLastDays = completedTasksLastDays;
            ReportGeneratedAt = DateTime.UtcNow;
            DaysInReport = daysInReport;
        }

        // Métricas calculadas
        public double TasksCompletedPerDay =>
            DaysInReport > 0 ? Math.Round((double)CompletedTasksLastDays / DaysInReport, 2) : 0;

        public double EfficiencyScore =>
            CalculateEfficiencyScore();

        private double CalculateEfficiencyScore()
        {
            if (TotalTasks == 0)
                return 0;

            double completionFactor = CompletionRate / 100.0;
            double timeFactor = AverageCompletionTimeInDays > 0
                ? Math.Min(1.0, 7.0 / AverageCompletionTimeInDays)
                : 0;

            return Math.Round((completionFactor * 0.7 + timeFactor * 0.3) * 100, 2);
        }
    }
}
