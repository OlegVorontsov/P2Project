using FluentValidation;
using P2Project.Core.Errors;
using P2Project.Core.Validation;

namespace P2Project.Volunteers.Application.Commands.HardDelete;

public class HardDeleteValidator :
    AbstractValidator<HardDeleteCommand>
{
    public HardDeleteValidator()
    {
        RuleFor(d => d.VolunteerId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}