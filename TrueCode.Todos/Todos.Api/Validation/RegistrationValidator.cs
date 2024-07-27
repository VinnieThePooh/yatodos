using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Todos.DataAccess.Identity;
using TrueCode.Todos.Models;

namespace TrueCode.Todos.Validation;

public class RegistrationValidator : AbstractValidator<CustomRegisterRequest>
{
    private readonly IServiceProvider _provider;

    //todo: potentially rules should be exposed according to IdentityOptions
    public RegistrationValidator(IServiceProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password should not be empty");

        RuleFor(x => x.PasswordConfirmation).NotEmpty().WithMessage("Password confirmation should not be empty");
        RuleFor(x => x.PasswordConfirmation).Must(MatchWithPassword).WithMessage("Password and its confirmation should match");

        RuleFor(x => x.Email).Matches(x => new Regex("^[^\\s@]+@([^\\s@.,]+\\.)+[^\\s@.,]{2,}$")).WithMessage("Email is incorrect");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email should not be empty");
        RuleFor(x => x.Email).MustAsync(BeUnique).WithMessage("Email already exists");
    }

    private async Task<bool> BeUnique(string email, CancellationToken token = default)
    {
        await using var scope = _provider.CreateAsyncScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        return await userManager.FindByEmailAsync(email) is null;
    }

    private bool MatchWithPassword(CustomRegisterRequest request, string confirmation) =>
        string.Equals(request.Password, confirmation, StringComparison.InvariantCulture);
}