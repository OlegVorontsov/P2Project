using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Accounts.Domain;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Application.Commands.EmailManagement.GenerateEmailConfirmationToken;

public class GenerateEmailConfirmationTokenHandler(
    UserManager<User> userManager,
    IAccountsReadDbContext accountsReadDbContext)
    : ICommandHandler<string, GenerateEmailConfirmationTokenCommand>
{
    public async Task<Result<string, ErrorList>> Handle(
        GenerateEmailConfirmationTokenCommand command,
        CancellationToken ct)
    {
        var getUserResult = await accountsReadDbContext.Users.FirstOrDefaultAsync(
            u => u.Id == command.UserId, ct);
        if (getUserResult is null)
            return Errors.General.NotFound(command.UserId).ToErrorList();

        var user = await userManager.FindByIdAsync(getUserResult.Id.ToString());
        if (user is null)
            return Errors.General.NotFound(command.UserId).ToErrorList();
        
        return await userManager.GenerateEmailConfirmationTokenAsync(user);
    }
}