using System.ComponentModel.DataAnnotations;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs.Activity
{
    /// <summary>
    /// Objeto de transferência de dados para criação de uma nova atividade dentro de um projeto.
    /// </summary>
    public class CreateActivityDto
    {
        /// <summary>
        /// Título da atividade.
        /// </summary>
        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres.")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Descrição detalhada da atividade.
        /// </summary>
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Data limite para conclusão da atividade.
        /// </summary>
        [Required(ErrorMessage = "A data de vencimento é obrigatória.")]
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Prioridade atribuída à atividade (Low, Medium, High).
        /// </summary>
        [Required(ErrorMessage = "A prioridade é obrigatória.")]
        [EnumDataType(typeof(ActivityPriority), ErrorMessage = "Prioridade inválida.")]
        public ActivityPriority Priority { get; set; }

        /// <summary>
        /// Identificador do projeto ao qual a atividade está vinculada.
        /// </summary>
        [Required(ErrorMessage = "O ID do projeto é obrigatório.")]
        public Guid ProjectId { get; set; }
    }
}
