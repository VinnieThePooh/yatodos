using System.Data;
using FluentValidation;
using Todos.Models.Domain;
using TrueCode.Todos.Models;

namespace TrueCode.Todos.Validation;

public class CreateRequestValidator : AbstractValidator<CreateTodoRequest>
{
    public CreateRequestValidator()
    {
        RuleFor(x => x.Title).NotNull();
        RuleFor(x => x.Priority).InclusiveBetween((int)PriorityLevel.Relaxed, (int)PriorityLevel.SuperUrgent);
    }
}