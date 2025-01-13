using FluentValidation;
using P2Project.Core.Errors;
using P2Project.Core.Validation;

namespace P2Project.Volunteers.Application.Commands.ChangePetMainPhoto;

public class ChangePetMainPhotoValidator :
    AbstractValidator<ChangePetMainPhotoCommand>
{
    public ChangePetMainPhotoValidator()
    {
        RuleFor(p => p.VolunteerId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(p => p.PetId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(p => p.ObjectName)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}