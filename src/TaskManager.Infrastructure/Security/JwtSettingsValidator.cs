using Microsoft.Extensions.Options;
using TaskManager.Application.Settings;

namespace TaskManager.Infrastructure.Security
{
    public class JwtSettingsValidator : IValidateOptions<JwtSettings>
    {
        public ValidateOptionsResult Validate(string? name, JwtSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Key))
                return ValidateOptionsResult.Fail("JWT Key não pode ser nula ou vazia.");

            if (settings.Key.Length < 32)
                return ValidateOptionsResult.Fail("JWT Key deve ter no mínimo 32 caracteres.");

            return ValidateOptionsResult.Success;
        }
    }
}
