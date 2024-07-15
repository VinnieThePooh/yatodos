using FluentValidation;
using Todos.Models.Domain;
using TrueCode.Todos.Models;

namespace TrueCode.Todos.Validation;

public class UpdatePriorityValidator : AbstractValidator<UpdatePriorityRequest>
{
    public UpdatePriorityValidator()
    {
        RuleFor(x => x.UserId).NotEqual(0);
        RuleFor(x => x.TodoId).NotEqual(0);
        RuleFor(x => x.Priority).InclusiveBetween((int)PriorityLevel.Relaxed, (int)PriorityLevel.SuperUrgent);
    }
}