using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Application.Commands.SetAvatar.CompleteSetAvatar;

public class CompleteSetAvatarValidator :
    AbstractValidator<CompleteSetAvatarCommand>
{
    public CompleteSetAvatarValidator()
    {
        RuleFor(ap => ap.Key)
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