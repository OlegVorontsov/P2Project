using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.Discussions.Application.DiscussionsManagement.Queries.GetById;

public class GetByIdValidator :
    AbstractValidator<GetByIdQuery>
{
    public GetByIdValidator()
    {
        RuleFor(d => d.DiscussionId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}