using TrueCode.Todos.Auth;

namespace TrueCode.Todos.Models;

public struct ConfigSnapshot
{
    public JwtSettings Jwt { get; set; }

    public CorsSettings Cors { get; set; }

    public string DefaultConnectionString { get; set; }

    public string Environment { get; set; }
}