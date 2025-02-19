using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.Discussions.Application.DiscussionsManagement.Commands.EditMessage;

public class EditMessageValidator :
    AbstractValidator<EditMessageCommand>
{
    public EditMessageValidator()
    {
        RuleFor(r => r.SenderId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(r => r.DiscussionId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(r => r.MessageId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(r => r.MessageText)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}