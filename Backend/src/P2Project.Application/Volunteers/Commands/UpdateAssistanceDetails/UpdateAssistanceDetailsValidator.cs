using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.PetManagment.ValueObjects.Common;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Volunteers.Commands.UpdateAssistanceDetails
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
