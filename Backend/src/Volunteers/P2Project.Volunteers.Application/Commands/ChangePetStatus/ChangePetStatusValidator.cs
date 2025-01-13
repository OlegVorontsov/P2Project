using FluentValidation;
using P2Project.Core.Errors;
using P2Project.Core.Validation;
using P2Project.Volunteers.Domain.ValueObjects.Pets;

namespace P2Project.Volunteers.Application.Commands.ChangePetStatus;

public class ChangePetStatusValidator :
    AbstractValidator<ChangePetStatusCommand>
{
    public ChangePetStatusValidator()
    {
        RuleFor(p => p.VolunteerId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(p => p.PetId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(p => p.Status).MustBeValueObject(
            AssistanceStatus.Create);
    }
}