using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Volunteers.Commands.SoftDeletePet;

public class SoftDeletePetValidator :
    AbstractValidator<SoftDeletePetCommand>
{
    public SoftDeletePetValidator()
    {
        RuleFor(p => p.VolunteerId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(p => p.PetId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}