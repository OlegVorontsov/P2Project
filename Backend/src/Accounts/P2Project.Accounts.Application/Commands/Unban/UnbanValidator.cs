using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Application.Commands.Unban;

public class UnbanValidator :
    AbstractValidator<UnbanCommand>
{
    public UnbanValidator()
    {
        RuleFor(ap => ap.UserId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}