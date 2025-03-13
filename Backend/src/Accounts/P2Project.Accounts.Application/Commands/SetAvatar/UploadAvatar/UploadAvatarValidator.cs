using FluentValidation;
using P2Project.Core.Dtos.Validators;

namespace P2Project.Accounts.Application.Commands.SetAvatar.UploadAvatar;

public class UploadAvatarValidator :
    AbstractValidator<UploadAvatarCommand>
{
    public UploadAvatarValidator()
    {
        RuleFor(u => u.StartMultipartUploadRequest).SetValidator(
            new StartMultipartUploadRequestValidator());
    }
}