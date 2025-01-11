using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Volunteers.Commands.HardDelete;

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