using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using TaskManager.Application.Settings;

namespace TaskManager.Infrastructure.Security
{
    public class JwtBearerOptionsSetup : IConfigureOptions<JwtBearerOptions>
    {
        private readonly JwtSettings _settings;

        public JwtBearerOptionsSetup(IOptions<JwtSettings> options)
        {
            _settings = options.Value;
        }

        public void Configure(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _settings.Issuer,
                ValidAudience = _settings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key)),
                NameClaimType = ClaimTypes.NameIdentifier
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = ctx =>
                {
                    Console.WriteLine($"Auth error: {ctx.Exception.Message}");
                    return Task.CompletedTask;
                }
            };
        }
    }
}
