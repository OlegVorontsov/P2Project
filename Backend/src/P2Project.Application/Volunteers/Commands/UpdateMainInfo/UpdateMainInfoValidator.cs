using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.PetManagment;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.PetManagment.ValueObjects.Common;
using P2Project.Domain.PetManagment.ValueObjects.Volunteers;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Volunteers.Commands.UpdateMainInfo
{
    public class UpdateMainInfoValidator :
        AbstractValidator<UpdateMainInfoCommand>
    {
        public UpdateMainInfoValidator()
        {
            RuleFor(i => i.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleFor(i => i.FullName)
                .MustBeValueObject(fn => FullName.Create(
                                                  fn.FirstName,
                                                  fn.SecondName,
                                                  fn.LastName));

            RuleFor(i => i.VolunteerInfo)
                .MustBeValueObject(vi => VolunteerInfo.Create(
                    vi.Age, vi.Grade));

            RuleFor(c => c.Gender).IsEnumName(typeof(Gender))
                .WithError(Errors.General.ValueIsInvalid("Gender"));

            RuleFor(i => i.Description)
                .MustBeValueObject(Description.Create);
        }
    }
}
