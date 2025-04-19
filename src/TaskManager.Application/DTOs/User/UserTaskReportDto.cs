using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs.User
{
    /// <summary>
    /// DTO utilizado para representar o relatório de desempenho de um usuário.
    /// </summary>
    public class UserTaskReportDto
    {
        /// <summary>
        /// Nome do usuário.
        /// </summary>
        [Required]
        public string? UserName { get; set; }

        /// <summary>
        /// Endereço de e-mail do usuário.
        /// </summary>
        [Required]
        public string? UserEmail { get; set; }

        /// <summary>
        /// Total de tarefas concluídas pelo usuário nos últimos 30 dias.
        /// </summary>
        public int TotalCompletedTasks { get; set; }

        /// <summary>
        /// Média de tarefas concluídas por dia nos últimos 30 dias.
        /// </summary>
        public double AverageCompletedTasksPerDay { get; set; }
    }
}
