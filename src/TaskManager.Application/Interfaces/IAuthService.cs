using TaskManager.Application.DTOs.User;

namespace TaskManager.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthDto> RegisterAsync(RegisterDto request);
        Task<AuthDto> LoginAsync(LoginDto request);
    }
}
