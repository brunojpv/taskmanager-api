namespace TaskManager.Application.DTOs.Project
{
    /// <summary>
    /// DTO utilizado para atualização das informações de um projeto existente.
    /// </summary>
    public class UpdateProjectDto
    {
        /// <summary>
        /// Nome atualizado do projeto.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Descrição atualizada do projeto.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Identificador do usuário responsável pelo projeto.
        /// </summary>
        public Guid UserId { get; set; }
    }
}
