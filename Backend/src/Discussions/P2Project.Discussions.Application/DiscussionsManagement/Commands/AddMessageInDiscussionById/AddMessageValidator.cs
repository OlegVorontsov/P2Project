using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.Discussions.Application.DiscussionsManagement.Commands.AddMessageInDiscussionById;

public class AddMessageValidator :
    AbstractValidator<AddMessageCommand>
{
    public AddMessageValidator()
    {
        RuleFor(r => r.SenderId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(r => r.DiscussionId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(r => r.Message)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}