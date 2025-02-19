using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.Discussions.Application.DiscussionsManagement.Commands.CreateMessage;

public class CreateMessageValidator :
    AbstractValidator<CreateMessageCommand>
{
    public CreateMessageValidator()
    {
        RuleFor(r => r.SenderId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(r => r.ParticipantId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(r => r.Message)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}