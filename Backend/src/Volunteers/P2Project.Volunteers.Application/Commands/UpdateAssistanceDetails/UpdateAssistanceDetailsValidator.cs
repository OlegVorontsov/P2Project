using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.ValueObjects;

namespace P2Project.Volunteers.Application.Commands.UpdateAssistanceDetails
{
    public class UpdateAssistanceDetailsValidator :
        AbstractValidator<UpdateAssistanceDetailsCommand>
    {
        public UpdateAssistanceDetailsValidator()
        {
            RuleFor(a => a.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleForEach(a => a.AssistanceDetails)
                .MustBeValueObject(ad =>
                                AssistanceDetail.Create(
                                    ad.Name,
                                    ad.Description,
                                    ad.AccountNumber));
        }
    }
}
