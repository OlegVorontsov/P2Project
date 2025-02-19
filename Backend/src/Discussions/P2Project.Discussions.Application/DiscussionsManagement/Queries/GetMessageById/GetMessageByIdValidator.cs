using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.Discussions.Application.DiscussionsManagement.Queries.GetMessageById;

public class GetMessageByIdValidator :
    AbstractValidator<GetMessageByIdQuery>
{
    public GetMessageByIdValidator()
    {
        RuleFor(d => d.MessageId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}