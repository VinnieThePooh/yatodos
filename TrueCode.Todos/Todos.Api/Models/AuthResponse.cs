namespace TrueCode.Todos.Models;

public record struct AuthResponse(string Token, UserProfile profile);
public record struct UserProfile(int UserId, string Name, string Email);
