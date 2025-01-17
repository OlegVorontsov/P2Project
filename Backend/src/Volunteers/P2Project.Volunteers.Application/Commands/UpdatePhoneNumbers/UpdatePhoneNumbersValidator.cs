using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.ValueObjects;

namespace P2Project.Volunteers.Application.Commands.UpdatePhoneNumbers
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
