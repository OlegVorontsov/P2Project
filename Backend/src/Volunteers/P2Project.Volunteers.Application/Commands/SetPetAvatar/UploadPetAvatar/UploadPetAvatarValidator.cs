using FluentValidation;
using P2Project.Core.Dtos.Validators;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.Volunteers.Application.Commands.SetPetAvatar.UploadPetAvatar;

public class UploadPetAvatarValidator :
    AbstractValidator<UploadPetAvatarCommand>
{
    public UploadPetAvatarValidator()
    {
        RuleFor(u => u.VolunteerId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(u => u.PetId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(u => u.StartMultipartUploadRequest).SetValidator(
            new StartMultipartUploadRequestValidator());
    }
}