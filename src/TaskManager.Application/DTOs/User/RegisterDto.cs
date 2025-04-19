using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs.User
{
    /// <summary>
    /// DTO utilizado para registro de novos usuários na aplicação.
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// Nome completo do usuário.
        /// </summary>
        [Required]
        public string? Name { get; set; }

        /// <summary>
        /// Endereço de e-mail do usuário.
        /// </summary>
        [Required]
        public string? Email { get; set; }

        /// <summary>
        /// Senha definida pelo usuário para acesso.
        /// </summary>
        [Required]
        public string? Password { get; set; }
    }
}
