using FluentValidation;
using P2Project.Core.Dtos.Validators;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Application.Commands.SetAvatar.UploadAvatar;

public class UploadAvatarValidator :
    AbstractValidator<UploadAvatarCommand>
{
    public UploadAvatarValidator()
    {
        RuleFor(u => u.UserId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(u => u.StartMultipartUploadRequest).SetValidator(
            new StartMultipartUploadRequestValidator());
    }
}