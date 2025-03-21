using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Agreements;
using P2Project.Core;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;
using P2Project.VolunteerRequests.Application.Interfaces;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.SetApprovedStatus;

public class SetApprovedStatusHandler :
    ICommandHandler<Guid, SetApprovedStatusCommand>
{
    private readonly IValidator<SetApprovedStatusCommand> _validator;
    private readonly IAccountsAgreements _accountsAgreements;
    private readonly IPublisher _publisher;
    private readonly IVolunteerRequestsRepository _volunteerRequestsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SetApprovedStatusHandler> _logger;

    public SetApprovedStatusHandler(
        IValidator<SetApprovedStatusCommand> validator,
        IAccountsAgreements accountsAgreements,
        IPublisher publisher,
        IVolunteerRequestsRepository volunteerRequestsRepository,
        [FromKeyedServices(Modules.VolunteerRequests)] IUnitOfWork unitOfWork,
        ILogger<SetApprovedStatusHandler> logger)
    {
        _validator = validator;
        _publisher = publisher;
        _volunteerRequestsRepository = volunteerRequestsRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _accountsAgreements = accountsAgreements;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        SetApprovedStatusCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var existedRequest = await _volunteerRequestsRepository.GetById(
            command.RequestId, cancellationToken);
        if (existedRequest.IsFailure)
            return Errors.General.NotFound(command.RequestId).ToErrorList();
        
        //todo event
        var volunteerAccountResult = await _accountsAgreements
            .CreateVolunteerAccountForUser(existedRequest.Value.UserId, cancellationToken);
        if (volunteerAccountResult.IsFailure)
            return volunteerAccountResult.Error;
        
        existedRequest.Value.SetApprovedStatus(command.AdminId, command.Comment);
        
        await _publisher.PublishDomainEvents(existedRequest.Value, cancellationToken);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation(
            "Volunteer request with id {requestId} was approved", command.RequestId);

        return existedRequest.Value.Id.Value;
    }
}