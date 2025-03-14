using FilesService.Core.Dtos;
using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.Core.Validation
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
                .Must(s => s.Length < 10000000);
        }
    }
}