using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs.Activity
{
    /// <summary>
    /// Dados completos de uma atividade retornados pela API.
    /// </summary>
    public class ActivityDto
    {
        /// <summary>
        /// Identificador único da atividade.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Título da atividade.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Descrição detalhada da atividade.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Data de vencimento da atividade.
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Status atual da atividade (Pending, InProgress, Completed).
        /// </summary>
        public ActivityStatus Status { get; set; }

        /// <summary>
        /// Prioridade atribuída à atividade (Low, Medium, High).
        /// </summary>
        public ActivityPriority Priority { get; set; }

        /// <summary>
        /// Data de criação da atividade.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Identificador do projeto ao qual a atividade está vinculada.
        /// </summary>
        public Guid ProjectId { get; set; }
    }
}
