using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.Volunteers.Application.Commands.SetPetAvatar.CompleteSetPetAvatar;

public class CompleteSetPetAvatarValidator :
    AbstractValidator<CompleteSetPetAvatarCommand>
{
    public CompleteSetPetAvatarValidator()
    {
        RuleFor(ap => ap.VolunteerId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(ap => ap.PetId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(ap => ap.FileName)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(ap => ap.BucketName)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(ap => ap.UploadId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
            
        RuleFor(ap => ap.ETag)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}