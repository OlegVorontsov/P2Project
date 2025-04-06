using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Accounts.Domain;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Application.Commands.EmailManagement.ConfirmEmail;

public class ConfirmEmailHandler(
    UserManager<User> userManager,
    IAccountsReadDbContext accountsReadDbContext)
    : ICommandHandler<ConfirmEmailCommand>
{
    public async Task<UnitResult<ErrorList>> Handle(
        ConfirmEmailCommand command,
        CancellationToken ct)
    {
        var getUserResult = await accountsReadDbContext.Users.FirstOrDefaultAsync(
            u => u.Id == command.UserId, ct);
        if (getUserResult is null)
            return Errors.General.NotFound(command.UserId).ToErrorList();

        var user = await userManager.FindByIdAsync(getUserResult.Id.ToString());
        if (user is null)
            return Errors.General.NotFound(command.UserId).ToErrorList();
        
        var result = await userManager.ConfirmEmailAsync(user, command.Token);
        return result.Succeeded
            ? Result.Success<ErrorList>()
            : Errors.General.Failure("Не удалось подтвердить почту").ToErrorList();
    }
}