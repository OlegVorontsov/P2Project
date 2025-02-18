using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.SetReopenStatus;

public class SetReopenStatusValidator :
    AbstractValidator<SetReopenStatusCommand>
{
    public SetReopenStatusValidator()
    {
        RuleFor(r => r.UserId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(r => r.RequestId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(r => r.Comment)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}