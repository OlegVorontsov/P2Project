using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.ValueObjects;
using P2Project.Volunteers.Domain;
using P2Project.Volunteers.Domain.ValueObjects.Volunteers;

namespace P2Project.Volunteers.Application.Commands.UpdateMainInfo
{
    public class UpdateMainInfoValidator :
        AbstractValidator<UpdateMainInfoCommand>
    {
        public UpdateMainInfoValidator()
        {
            RuleFor(i => i.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

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
