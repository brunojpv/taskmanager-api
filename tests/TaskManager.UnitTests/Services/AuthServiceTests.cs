using Microsoft.Extensions.Options;
using NSubstitute;
using TaskManager.Application.DTOs.User;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Services;
using TaskManager.Application.Settings;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.UnitTests.Services
{
    public class AuthServiceTests
    {
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private readonly JwtSettings _jwtSettings = new JwtSettings
        {
            Key = "super_secret_test_key_123",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpirationHours = 2
        };

        private IAuthService CreateService() =>
            new AuthService(_userRepository, Options.Create(_jwtSettings));

        [Fact]
        public async Task RegisterAsync_ShouldCreateUserAndReturnToken()
        {
            var dto = new RegisterDto
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "password"
            };

            _userRepository.GetByEmailAsync(dto.Email).Returns((User)null!);

            var service = CreateService();
            var result = await service.RegisterAsync(dto);

            Assert.False(string.IsNullOrWhiteSpace(result.Token));
            await _userRepository.Received(1).AddAsync(Arg.Any<User>());
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsValid()
        {
            var hashed = BCrypt.Net.BCrypt.HashPassword("password");
            var user = User.Create("Test User", "test@example.com", hashed);
            var dto = new LoginDto { Email = user.Email, Password = "password" };

            _userRepository.GetByEmailAsync(user.Email).Returns(user);

            var service = CreateService();
            var result = await service.LoginAsync(dto);

            Assert.False(string.IsNullOrWhiteSpace(result.Token));
        }

        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenCredentialsInvalid()
        {
            var user = User.Create("Test User", "test@example.com", "correct-password");
            var dto = new LoginDto { Email = user.Email, Password = "wrong-password" };

            _userRepository.GetByEmailAsync(user.Email).Returns(user);
            var service = CreateService();

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.LoginAsync(dto));
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrow_WhenEmailAlreadyExists()
        {
            var dto = new RegisterDto
            {
                Name = "User",
                Email = "exists@example.com",
                Password = "123456"
            };

            _userRepository.GetByEmailAsync(dto.Email).Returns(User.Create(dto.Name, dto.Email, dto.Password));
            var service = CreateService();

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.RegisterAsync(dto));
        }
    }
}
