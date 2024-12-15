using FluentValidation;
using P2Project.Application.Shared.Dtos.Validators;
using P2Project.Application.Validation;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Volunteers.Commands.UploadFilesToPet
{
    public class UploadFilesToPetValidator :
        AbstractValidator<UploadFilesToPetCommand>
    {
        public UploadFilesToPetValidator()
        {
            RuleFor(u => u.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleFor(u => u.PetId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleForEach(u => u.Files).SetValidator(
                new UploadFileDtoValidator());
        }
    }
}
