namespace TaskManager.Application.Settings
{
    /// <summary>
    /// Representa as configurações JWT carregadas do appsettings.json
    /// </summary>
    public class JwtSettings
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpirationHours { get; set; } = 2;
    }
}
