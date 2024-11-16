using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;

namespace P2Project.Application.Volunteers.UpdateAssistanceDetails
{
    public class UpdateAssistanceDetailsValidator :
        AbstractValidator<UpdateAssistanceDetailsRequest>
    {
        public UpdateAssistanceDetailsValidator()
        {
            RuleFor(a => a.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());
        }
    }

    public class UpdateAssistanceDetailsDtoValidator :
    AbstractValidator<UpdateAssistanceDetailsDto>
    {
        public UpdateAssistanceDetailsDtoValidator()
        {
            RuleForEach(a => a.AssistanceDetails).MustBeValueObject(ad =>
                                            AssistanceDetail.Create(
                                                ad.Name,
                                                ad.Description,
                                                ad.AccountNumber));
        }
    }
}
