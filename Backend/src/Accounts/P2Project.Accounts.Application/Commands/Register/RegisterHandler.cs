using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Accounts.Domain;
using P2Project.Accounts.Domain.Accounts;
using P2Project.Accounts.Domain.Events;
using P2Project.Accounts.Domain.RolePermission.Roles;
using P2Project.Core;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.Core.Interfaces.Outbox;
using P2Project.Core.Outbox.Messages.Accounts;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.ValueObjects;

namespace P2Project.Accounts.Application.Commands.Register;

public class RegisterHandler(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IAccountsManager accountManager,
    IOutboxRepository outboxRepository,
    [FromKeyedServices(Modules.Accounts)] IUnitOfWork unitOfWork,
    ILogger<RegisterHandler> logger,
    IPublisher publisher) : ICommandHandler<string, RegisterCommand>
{
    public async Task<Result<string, ErrorList>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = await unitOfWork.BeginTransaction(cancellationToken);
        try
        {
            var existingUser = await userManager.FindByEmailAsync(command.Email);
            if(existingUser != null)
                return Errors.General.AlreadyExists().ToErrorList();

            var participantRole = await roleManager.FindByNameAsync(ParticipantAccount.RoleName) 
                                  ?? throw new ApplicationException("Participant role is not found");
        
            var userName = FullName.Create(
                command.FullName.FirstName,
                command.FullName.SecondName,
                command.FullName.LastName).Value;
        
            var userResult = User.Create(
                command.Email, command.UserName, userName, participantRole);
            if(userResult.IsFailure)
                return Errors.General.Failure("User").ToErrorList();
        
            var result = await userManager.CreateAsync(userResult.Value, command.Password);
            var participantAccount = new ParticipantAccount(userResult.Value);
            await accountManager
                .CreateParticipantAccount(participantAccount, cancellationToken);
        
            userResult.Value.ParticipantAccount = participantAccount;
        
            await userManager.UpdateAsync(userResult.Value);
            
            await unitOfWork.SaveChanges(cancellationToken);

            var createdUserEvent = new CreatedUserEvent(
                userResult.Value.Id,
                command.TelegramUserId,
                userResult.Value.Email,
                userResult.Value.UserName,
                participantRole.Name);
            
            await outboxRepository.Add(createdUserEvent, cancellationToken);
            
            transaction.Commit();

            await publisher.Publish(new UserWasRegisteredEvent(userResult.Value.Id), cancellationToken);

            logger.LogInformation("User {username} was registered", userResult.Value.UserName);
            return $"{userResult.Value.UserName} was registered";
        }
        catch (Exception e)
        {
            logger.LogError("Failed to register user {username}", command.UserName);
            transaction.Rollback();
        
            return Error.Failure("could.not.register.user", e.Message).ToErrorList();
        }
    }
}