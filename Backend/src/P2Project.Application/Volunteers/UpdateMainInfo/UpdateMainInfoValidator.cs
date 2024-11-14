using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;

namespace P2Project.Application.Volunteers.UpdateMainInfo
{
    public class UpdateMainInfoValidator :
        AbstractValidator<UpdateMainInfoRequest>
    {
        public UpdateMainInfoValidator()
        {
            RuleFor(i => i.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());
        }
    }
    public class UpdateMainInfoDtoValidator :
        AbstractValidator<UpdateMainInfoDto>
    {
        public UpdateMainInfoDtoValidator()
        {
            RuleFor(i => i.FullName).MustBeValueObject(fn =>
                    FullName.Create(
                        fn.FirstName,
                        fn.SecondName,
                        fn.LastName));

            RuleFor(i => i.Description).MustBeValueObject(Description.Create)
                .When(i => !string.IsNullOrWhiteSpace(i.Description));
        }
    }
}
