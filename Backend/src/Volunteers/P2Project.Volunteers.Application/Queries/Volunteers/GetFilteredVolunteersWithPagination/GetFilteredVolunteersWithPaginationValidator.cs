using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.Volunteers.Application.Queries.Volunteers.GetFilteredVolunteersWithPagination;

public class GetFilteredVolunteersWithPaginationValidator :
    AbstractValidator<GetFilteredVolunteersWithPaginationQuery>
{
    public GetFilteredVolunteersWithPaginationValidator()
    {
        RuleFor(q => q.Page)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("Page"));

        RuleFor(q => q.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("PageSize"));
    }
}