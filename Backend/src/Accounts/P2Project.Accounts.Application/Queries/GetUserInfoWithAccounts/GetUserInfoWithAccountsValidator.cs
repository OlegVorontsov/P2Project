using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Application.Queries.GetUserInfoWithAccounts;

public class GetUserInfoWithAccountsValidator :
    AbstractValidator<GetUserInfoWithAccountsQuery>
{
    public GetUserInfoWithAccountsValidator()
    {
        RuleFor(p => p.Id)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}