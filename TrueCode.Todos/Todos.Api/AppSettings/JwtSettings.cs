namespace TrueCode.Todos.Auth;

internal class JwtSettings
{
    public const string SettingsKey = "Jwt";
    
    public string PrivateKey { get; set; }

    public string Audience { get; set; }

    public string Issuer { get; set; }

    public TimeSpan AccessTokenExpiration { get; set; } = TimeSpan.FromMinutes(60);

    public TimeSpan RefreshTokenExpiration { get; set; } = TimeSpan.FromMinutes(120);
}