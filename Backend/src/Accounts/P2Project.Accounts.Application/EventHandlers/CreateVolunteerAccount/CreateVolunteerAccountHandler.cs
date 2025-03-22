using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Accounts.Domain;
using P2Project.Accounts.Domain.Accounts;
using P2Project.Accounts.Domain.RolePermission.Roles;
using P2Project.Core.Events;
using P2Project.SharedKernel.ValueObjects;

namespace P2Project.Accounts.Application.EventHandlers.CreateVolunteerAccount;

public class CreateVolunteerAccountHandler :
    INotificationHandler<CreateVolunteerAccountEvent>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IAccountsManager _accountManager;
    private readonly ILogger<CreateVolunteerAccountHandler> _logger;

    public CreateVolunteerAccountHandler(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IAccountsManager accountManager,
        ILogger<CreateVolunteerAccountHandler> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _accountManager = accountManager;
        _logger = logger;
    }

    public async Task Handle(
        CreateVolunteerAccountEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(domainEvent.UserId.ToString());
        if (user == null)
            throw new Exception("User not found");
        
        var workingExperience = Experience.Create(0).Value;
        
        var volunteerRole = await _roleManager.FindByNameAsync(VolunteerAccount.RoleName)
                            ?? throw new ApplicationException("Volunteer role isn't found");
        
        var volunteerAccount = new VolunteerAccount(user, workingExperience);
        
        user.VolunteerAccount = volunteerAccount;
        
        user.AddRole(volunteerRole);
        
        var result = await _accountManager
            .CreateVolunteerAccount(volunteerAccount, cancellationToken);
        
        if (result.IsFailure)
            throw new Exception("Fail to create volunteer account");
        
        _logger.LogInformation(
            "Volunteer account was was created for user {userId}",
            domainEvent.UserId);
    }
}