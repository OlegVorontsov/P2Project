using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.Shared;

namespace P2Project.Application.Volunteers.UploadFilesToPet
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
        }
    }
}
