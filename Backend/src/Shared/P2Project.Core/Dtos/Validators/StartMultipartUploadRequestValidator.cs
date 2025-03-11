using FilesService.Core.Requests.AmazonS3;
using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.Core.Dtos.Validators
{
    public class StartMultipartUploadRequestValidator :
        AbstractValidator<StartMultipartUploadRequest>
    {
        public StartMultipartUploadRequestValidator()
        {
            RuleFor(fd => fd.BucketName)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());
            
            RuleFor(fd => fd.FileName)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());
            
            RuleFor(fd => fd.ContentType)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleFor(fd => fd.Size)
                .Must(s => s < 10000000);
        }
    }
}
