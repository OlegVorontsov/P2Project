using FluentValidation;
using P2Project.Core.Events;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.Discussions.Application.DiscussionsManagement.EventHandlers.CreateMessage;

public class CreateMessageValidator :
    AbstractValidator<CreateMessageEvent>
{
    public CreateMessageValidator()
    {
        RuleFor(r => r.RequestId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(r => r.SenderId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(r => r.Message)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}