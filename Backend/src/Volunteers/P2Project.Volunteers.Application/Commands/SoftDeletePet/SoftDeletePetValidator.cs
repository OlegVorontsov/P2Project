using FluentValidation;
using P2Project.Core.Errors;
using P2Project.Core.Validation;

namespace P2Project.Volunteers.Application.Commands.SoftDeletePet;

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