using System.ComponentModel.DataAnnotations;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs.Activity
{
    /// <summary>
    /// DTO utilizado para atualização de uma atividade existente.
    /// </summary>
    public class UpdateActivityDto
    {
        /// <summary>
        /// Título atualizado da atividade.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Descrição atualizada da atividade.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Nova data limite para conclusão da atividade.
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Novo status da atividade (ex: Pendente, Em Progresso, Concluída).
        /// </summary>
        [EnumDataType(typeof(ActivityStatus))]
        public ActivityStatus Status { get; set; }

        /// <summary>
        /// Identificador do projeto ao qual a atividade pertence.
        /// </summary>
        public Guid ProjectId { get; set; }
    }
}
