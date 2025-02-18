using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Queries.GetAllByAdminId;

public class GetAllByAdminIdValidator :
    AbstractValidator<GetAllByAdminIdQuery>
{
    public GetAllByAdminIdValidator()
    {
        RuleFor(r => r.AdminId)
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