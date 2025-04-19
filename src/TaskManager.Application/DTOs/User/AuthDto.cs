namespace TaskManager.Application.DTOs.User
{
    /// <summary>
    /// DTO utilizado como resposta da autenticação de usuário.
    /// </summary>
    public class AuthDto
    {
        /// <summary>
        /// Token JWT gerado após autenticação bem-sucedida.
        /// </summary>
        public string? Token { get; set; }
    }
}
