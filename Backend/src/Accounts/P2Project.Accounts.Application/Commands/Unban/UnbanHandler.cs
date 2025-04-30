using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Agreements;
using P2Project.Accounts.Domain.Events;
using P2Project.Core;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Application.Commands.Unban;

public class UnbanHandler :
    ICommandHandler<Guid, UnbanCommand>
{
    private readonly IValidator<UnbanCommand> _validator;
    private readonly IAccountsAgreements _accountsAgreements;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UnbanHandler> _logger;
    private readonly IPublisher _publisher;

    public UnbanHandler(
        IValidator<UnbanCommand> validator,
        IAccountsAgreements accountsAgreements,
        [FromKeyedServices(Modules.Accounts)] IUnitOfWork unitOfWork,
        ILogger<UnbanHandler> logger,
        IPublisher publisher)
    {
        _validator = validator;
        _accountsAgreements = accountsAgreements;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _publisher = publisher;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UnbanCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        await _accountsAgreements.UnbanUser(command.UserId, cancellationToken);
        
        await _unitOfWork.SaveChanges(cancellationToken);

        await _publisher.Publish(new UserWasChangedEvent(), cancellationToken);

        _logger.LogInformation("User {userId} unbanned for volunteer requests",
            command.UserId);

        return command.UserId;
    }
}