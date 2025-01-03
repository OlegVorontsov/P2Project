using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.PetManagment.ValueObjects.Pets;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Volunteers.Commands.ChangePetStatus;

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