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

public class SetApprovedStatusHandler(
    IValidator<SetApprovedStatusCommand> validator,
    IPublisher publisher,
    IVolunteerRequestsRepository volunteerRequestsRepository,
    [FromKeyedServices(Modules.VolunteerRequests)]
    IUnitOfWork unitOfWork,
    ILogger<SetApprovedStatusHandler> logger) : ICommandHandler<Guid, SetApprovedStatusCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        SetApprovedStatusCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(
            command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var existedRequest = await volunteerRequestsRepository.GetById(
            command.RequestId, cancellationToken);
        if (existedRequest.IsFailure)
            return Errors.General.NotFound(command.RequestId).ToErrorList();
        
        existedRequest.Value.SetApprovedStatus(command.AdminId, command.Comment);
        
        await publisher.PublishDomainEvents(existedRequest.Value, cancellationToken);
        
        await unitOfWork.SaveChanges(cancellationToken);
        
        logger.LogInformation(
            "Volunteer request with id {requestId} was approved", command.RequestId);

        return existedRequest.Value.Id.Value;
    }
}