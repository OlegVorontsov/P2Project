﻿using FluentValidation;
using P2Project.Core.Dtos.Files;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.Core.Dtos.Validators
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
