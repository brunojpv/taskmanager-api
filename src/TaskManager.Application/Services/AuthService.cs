using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.Application.DTOs.User;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Settings;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IUserRepository userRepository, IOptions<JwtSettings> jwtOptions)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtOptions.Value;
        }

        public async Task<AuthDto> RegisterAsync(RegisterDto request)
        {
            var userExists = await _userRepository.GetByEmailAsync(request.Email);
            if (userExists is not null)
                throw new InvalidOperationException("E-mail já cadastrado");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = User.Create(request.Name, request.Email, hashedPassword);

            await _userRepository.AddAsync(user);
            return new AuthDto { Token = GenerateJwt(user) };
        }

        public async Task<AuthDto> LoginAsync(LoginDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new InvalidOperationException("Credenciais inválidas");

            return new AuthDto { Token = GenerateJwt(user) };
        }

        private string GenerateJwt(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
