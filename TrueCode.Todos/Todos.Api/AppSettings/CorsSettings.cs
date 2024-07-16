namespace TrueCode.Todos.Auth;

public class CorsSettings : ISettings
{
    public string AllowedOrigin { get; set; }

    public static string SectionKey => "Cors";
}