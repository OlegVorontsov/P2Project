using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.PetManagment;
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

            RuleFor(i => i.MainInfoDto.FullName)
                .MustBeValueObject(fn => FullName.Create(
                                                  fn.FirstName,
                                                  fn.SecondName,
                                                  fn.LastName));

            RuleFor(i => i.MainInfoDto.Age).InclusiveBetween(
                Constants.MIN_AGE, Constants.MAX_AGE)
                .WithError(Errors.General.ValueIsInvalid("Age"));

            RuleFor(c => c.MainInfoDto.Gender).IsEnumName(typeof(Gender))
                .WithError(Errors.General.ValueIsInvalid("Gender"));

            RuleFor(i => i.MainInfoDto.Description)
                .MustBeValueObject(Description.Create);
        }
    }
}
