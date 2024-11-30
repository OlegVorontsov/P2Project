using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;

namespace P2Project.Application.Volunteers.UpdatePhoneNumbers
{
    public class UpdatePhoneNumbersValidator :
        AbstractValidator<UpdatePhoneNumbersCommand>
    {
        public UpdatePhoneNumbersValidator()
        {
            RuleFor(p => p.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleForEach(p => p.PhoneNumbers)
                .MustBeValueObject(pn => PhoneNumber.Create(
                                                pn.Value,
                                                pn.IsMain));
        }
    }
}
