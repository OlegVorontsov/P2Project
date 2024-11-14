using FluentValidation;
using P2Project.Application.Validation;
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
}
