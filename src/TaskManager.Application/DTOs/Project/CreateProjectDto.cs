using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs.Project
{
    /// <summary>
    /// DTO utilizado para criação de um novo projeto.
    /// </summary>
    public class CreateProjectDto
    {
        /// <summary>
        /// Nome do projeto.
        /// </summary>
        [Required]
        public string? Name { get; set; }

        /// <summary>
        /// Descrição do projeto.
        /// </summary>
        [Required]
        public string? Description { get; set; }

        /// <summary>
        /// Identificador do usuário que está criando o projeto.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
    }
}
