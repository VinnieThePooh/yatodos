namespace TrueCode.Todos.Auth;

public class JwtSettings : ISettings
{
    public string PrivateKey { get; set; }

    public string Audience { get; set; }

    public string Issuer { get; set; }

    public TimeSpan AccessTokenExpiration { get; set; } = TimeSpan.FromMinutes(60);

    public TimeSpan RefreshTokenExpiration { get; set; } = TimeSpan.FromMinutes(120);

    public static string SectionKey => "Jwt";
}