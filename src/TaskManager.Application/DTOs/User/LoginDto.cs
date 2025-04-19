using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs.User
{
    /// <summary>
    /// DTO utilizado para autenticação de usuários.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Endereço de e-mail do usuário.
        /// </summary>
        [Required]
        public string? Email { get; set; }

        /// <summary>
        /// Senha do usuário.
        /// </summary>
        [Required]
        public string? Password { get; set; }
    }
}
