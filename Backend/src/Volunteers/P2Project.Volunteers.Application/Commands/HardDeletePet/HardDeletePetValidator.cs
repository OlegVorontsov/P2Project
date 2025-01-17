using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.Volunteers.Application.Commands.HardDeletePet;

public class HardDeletePetValidator :
    AbstractValidator<HardDeletePetCommand>
{
    public HardDeletePetValidator()
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