using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Accounts.Domain;
using P2Project.Accounts.Domain.Accounts;
using P2Project.Accounts.Domain.RolePermission.Roles;
using P2Project.Accounts.Domain.Users.ValueObjects;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Application.Commands.Register;

public class RegisterHandler :
    ICommandHandler<string, RegisterCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IAccountsManager _accountManager;
    private readonly ILogger<RegisterHandler> _logger;
    
    public RegisterHandler(
        UserManager<User> userManager,
        ILogger<RegisterHandler> logger, RoleManager<Role> roleManager, IAccountsManager accountManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _accountManager = accountManager;
        _logger = logger;
    }
    
    public async Task<Result<string, ErrorList>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken = default)
    {
        var existingUser = await _userManager.FindByEmailAsync(command.Email);
        if(existingUser != null)
            return Errors.General.AlreadyExists().ToErrorList();

        var participantRole = await _roleManager.FindByNameAsync(ParticipantAccount.RoleName) 
                              ?? throw new ApplicationException("Participant role is not found");
        
        var userName = FullName.Create(
            command.FullName.FirstName,
            command.FullName.SecondName,
            command.FullName.LastName).Value;
        
        var user = User.CreateParticipant(
            command.Email, command.UserName, userName, participantRole);
        
        var result = await _userManager.CreateAsync(user, command.Password);
        
        if (result.Succeeded)
        {
            var participantAccount = new ParticipantAccount(user);
            await _accountManager.CreateParticipantAccount(participantAccount);
        
            user.ParticipantAccount = participantAccount;
        
            await _userManager.UpdateAsync(user);
            
            _logger.LogInformation("User {username} was registered", user.UserName);
        }
        
        var errors = result.Errors
            .Select(e => Error.Failure(e.Code, e.Description)).ToList();
        
        return new ErrorList(errors);
    }
}