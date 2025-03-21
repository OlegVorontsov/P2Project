using FluentValidation;
using P2Project.Core.Events;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.Discussions.Application.DiscussionsManagement.EventHandlers.Create;

public class CreateDiscussionValidator :
    AbstractValidator<CreateDiscussionEvent>
{
    public CreateDiscussionValidator()
    {
        RuleFor(r => r.RequestId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(r => r.ReviewingUserId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(r => r.ApplicantUserId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}