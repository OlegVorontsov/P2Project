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
using P2Project.VolunteerRequests.Domain.Enums;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.SetReopenStatus;

public class SetReopenStatusHandler :
    ICommandHandler<Guid, SetReopenStatusCommand>
{
    private readonly IValidator<SetReopenStatusCommand> _validator;
    private readonly IAccountsAgreements _accountsAgreements;
    private readonly IVolunteerRequestsRepository _volunteerRequestsRepository;
    private readonly IPublisher _publisher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SetReopenStatusHandler> _logger;

    public SetReopenStatusHandler(
        IValidator<SetReopenStatusCommand> validator,
        IAccountsAgreements accountsAgreements,
        IVolunteerRequestsRepository volunteerRequestsRepository,
        IPublisher publisher,
        [FromKeyedServices(Modules.VolunteerRequests)] IUnitOfWork unitOfWork,
        ILogger<SetReopenStatusHandler> logger)
    {
        _validator = validator;
        _accountsAgreements = accountsAgreements;
        _volunteerRequestsRepository = volunteerRequestsRepository;
        _publisher = publisher;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        SetReopenStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var isUserBanned = await _accountsAgreements.IsUserBannedForVolunteerRequests(command.UserId, cancellationToken);
        if (isUserBanned)
            return Errors.General.Failure("user is banned").ToErrorList();
        
        var existedRequest = await _volunteerRequestsRepository.GetById(
            command.RequestId, cancellationToken);
        if (existedRequest.IsFailure)
            return Errors.General.NotFound(command.RequestId).ToErrorList();
        
        if (existedRequest.Value.Status == RequestStatus.Submitted)
            return Errors.General.Failure("already submitted").ToErrorList();
        
        existedRequest.Value.Refresh(command.UserId, command.Comment);
        
        await _publisher.PublishDomainEvents(existedRequest.Value, cancellationToken);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation(
            "Volunteer request with id {requestId} was reopened with submitted status",
            command.RequestId);

        return existedRequest.Value.Id.Value;
    }
}