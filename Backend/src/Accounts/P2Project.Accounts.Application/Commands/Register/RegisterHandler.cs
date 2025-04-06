using CSharpFunctionalExtensions;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Agreements.Messages;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Accounts.Domain;
using P2Project.Accounts.Domain.Accounts;
using P2Project.Accounts.Domain.RolePermission.Roles;
using P2Project.Core;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.ValueObjects;

namespace P2Project.Accounts.Application.Commands.Register;

public class RegisterHandler :
    ICommandHandler<string, RegisterCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IAccountsManager _accountManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterHandler> _logger;
    
    public RegisterHandler(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IAccountsManager accountManager,
        [FromKeyedServices(Modules.Accounts)] IUnitOfWork unitOfWork,
        ILogger<RegisterHandler> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _accountManager = accountManager;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<Result<string, ErrorList>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        try
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
        
            var userResult = User.Create(
                command.Email, command.UserName, userName, participantRole);
            if(userResult.IsFailure)
                return Errors.General.Failure("User").ToErrorList();
        
            var result = await _userManager.CreateAsync(userResult.Value, command.Password);
            var participantAccount = new ParticipantAccount(userResult.Value);
            await _accountManager
                .CreateParticipantAccount(participantAccount, cancellationToken);
        
            userResult.Value.ParticipantAccount = participantAccount;
        
            await _userManager.UpdateAsync(userResult.Value);
            
            await _unitOfWork.SaveChanges(cancellationToken);
            
            transaction.Commit();
            
            _logger.LogInformation("User {username} was registered", userResult.Value.UserName);
            return "User {username} was registered\", user.UserName";
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to register user {username}", command.UserName);
            transaction.Rollback();
        
            return Error.Failure("could.not.register.user", e.Message).ToErrorList();
        }
    }
}