using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs.ActivityComment
{
    /// <summary>
    /// DTO utilizado para adicionar um comentário a uma atividade.
    /// </summary>
    public class AddCommentDto
    {
        /// <summary>
        /// Identificador da atividade à qual o comentário será adicionado.
        /// </summary>
        [Required]
        public Guid ActivityId { get; set; }

        /// <summary>
        /// Conteúdo do comentário.
        /// </summary>
        [Required]
        public string? Content { get; set; }
    }
}
