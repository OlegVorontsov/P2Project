using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Queries.GetAllByUserId;

public class GetAllByUserIdValidator :
    AbstractValidator<GetAllByUserIdQuery>
{
    public GetAllByUserIdValidator()
    {
        RuleFor(r => r.UserId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(q => q.Page)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("Page"));

        RuleFor(q => q.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("PageSize"));
    }
}