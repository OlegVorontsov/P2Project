using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.Shared;

namespace P2Project.Application.Shared.Dtos.Validators
{
    public class UploadFileDtoValidator :
        AbstractValidator<UploadFileDto>
    {
        public UploadFileDtoValidator()
        {
            RuleFor(fd => fd.FileName)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleFor(fd => fd.Stream)
                .Must(s => s.Length < 5000000);
        }
    }
}
