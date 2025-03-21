using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Agreements;
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

    public UnbanHandler(
        IValidator<UnbanCommand> validator,
        IAccountsAgreements accountsAgreements,
        [FromKeyedServices(Modules.Accounts)] IUnitOfWork unitOfWork,
        ILogger<UnbanHandler> logger)
    {
        _validator = validator;
        _accountsAgreements = accountsAgreements;
        _unitOfWork = unitOfWork;
        _logger = logger;
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
        
        _logger.LogInformation("User {userId} unbanned for requests",
            command.UserId);

        return command.UserId;
    }
}