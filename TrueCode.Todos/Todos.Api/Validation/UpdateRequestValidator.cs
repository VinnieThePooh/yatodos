using FluentValidation;
using Todos.Models.Domain;
using TrueCode.Todos.Models;

namespace TrueCode.Todos.Validation;

public class UpdateRequestValidator : AbstractValidator<UpdateTodoRequest>
{
    public UpdateRequestValidator()
    {
        RuleFor(x => x.UserId).NotEqual(0);
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Priority).InclusiveBetween((int)PriorityLevel.Relaxed, (int)PriorityLevel.SuperUrgent);
    }
}