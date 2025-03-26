using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Accounts.Domain;
using P2Project.Accounts.Domain.Accounts;
using P2Project.Accounts.Domain.RolePermission.Roles;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.ValueObjects;

namespace P2Project.Accounts.Application.EventHandlers.CreateVolunteerAccount;

public class CreateVolunteerAccountHandler(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IAccountsManager accountManager,
    ILogger<CreateVolunteerAccountHandler> logger) : ICommandVoidHandler<CreateVolunteerAccountCommand>
{
    public async Task Handle(
        CreateVolunteerAccountCommand command,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.UserId.ToString());
        if (user == null)
            throw new Exception("User not found");
        
        var workingExperience = Experience.Create(0).Value;
        
        var volunteerRole = await roleManager.FindByNameAsync(VolunteerAccount.RoleName)
                            ?? throw new ApplicationException("Volunteer role isn't found");
        
        var volunteerAccount = new VolunteerAccount(user, workingExperience);
        
        user.VolunteerAccount = volunteerAccount;
        
        user.AddRole(volunteerRole);
        
        var result = await accountManager
            .CreateVolunteerAccount(volunteerAccount, cancellationToken);
        
        if (result.IsFailure)
            throw new Exception("Fail to create volunteer account");
        
        logger.LogInformation(
            "Volunteer account was was created for user {userId}",
            command.UserId);
    }
}