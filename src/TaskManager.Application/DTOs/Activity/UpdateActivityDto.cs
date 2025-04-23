using System.ComponentModel.DataAnnotations;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs.Activity
{
    /// <summary>
    /// Objeto de transferência de dados para atualização de uma atividade existente.
    /// </summary>
    public class UpdateActivityDto
    {
        /// <summary>
        /// Novo título da atividade.
        /// </summary>
        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres.")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Nova descrição da atividade.
        /// </summary>
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Nova data de vencimento da atividade.
        /// </summary>
        [Required(ErrorMessage = "A data de vencimento é obrigatória.")]
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Novo status da atividade (Pending, InProgress, Completed).
        /// </summary>
        [Required(ErrorMessage = "O status é obrigatório.")]
        [EnumDataType(typeof(ActivityStatus), ErrorMessage = "Status inválido.")]
        public ActivityStatus Status { get; set; }

        /// <summary>
        /// Nova prioridade da atividade (Low, Medium, High).
        /// </summary>
        [Required(ErrorMessage = "A prioridade é obrigatória.")]
        [EnumDataType(typeof(ActivityPriority), ErrorMessage = "Prioridade inválida.")]
        public ActivityPriority Priority { get; set; }

        /// <summary>
        /// Identificador do projeto associado à atividade.
        /// </summary>
        [Required(ErrorMessage = "O ID do projeto é obrigatório.")]
        public Guid ProjectId { get; set; }
    }
}
