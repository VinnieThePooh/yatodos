namespace TrueCode.Todos.Models;

/// <summary>
/// Simplified one-step register model
/// </summary>
public class CustomRegisterRequest
{
    public string Email { get; init; }

    /// <summary>
    /// The user's password.
    /// </summary>
    public string Password { get; init; }

    public string PasswordConfirmation { get; set; }
}