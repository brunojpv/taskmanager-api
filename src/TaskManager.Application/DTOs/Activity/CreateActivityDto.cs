using System.ComponentModel.DataAnnotations;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs.Activity
{
    /// <summary>
    /// DTO utilizado para criação de uma nova atividade dentro de um projeto.
    /// </summary>
    public class CreateActivityDto
    {
        /// <summary>
        /// Título da atividade.
        /// </summary>
        [Required]
        public string? Title { get; set; }

        /// <summary>
        /// Descrição detalhada da atividade.
        /// </summary>
        [Required]
        public string? Description { get; set; }

        /// <summary>
        /// Data limite para conclusão da atividade.
        /// </summary>
        [Required]
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Prioridade atribuída à atividade.
        /// </summary>
        [EnumDataType(typeof(ActivityPriority))]
        public ActivityPriority Priority { get; set; }

        /// <summary>
        /// Identificador do projeto ao qual a atividade está vinculada.
        /// </summary>
        [Required]
        public Guid ProjectId { get; set; }
    }
}
