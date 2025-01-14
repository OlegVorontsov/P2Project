using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

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