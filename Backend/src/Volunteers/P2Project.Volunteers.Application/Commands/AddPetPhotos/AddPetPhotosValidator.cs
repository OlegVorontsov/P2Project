using FluentValidation;
using P2Project.Core.Dtos.Validators;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.Volunteers.Application.Commands.AddPetPhotos
{
    public class AddPetPhotosValidator :
        AbstractValidator<AddPetPhotosCommand>
    {
        public AddPetPhotosValidator()
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
