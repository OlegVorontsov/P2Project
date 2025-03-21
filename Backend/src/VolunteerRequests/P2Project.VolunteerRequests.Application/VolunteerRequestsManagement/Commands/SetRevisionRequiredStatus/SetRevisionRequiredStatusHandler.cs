using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Core;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;
using P2Project.VolunteerRequests.Application.Interfaces;
using P2Project.VolunteerRequests.Domain.ValueObjects;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.SetRevisionRequiredStatus;

public class SetRevisionRequiredStatusHandler :
    ICommandHandler<Guid, SetRevisionRequiredStatusCommand>
{
    private readonly IValidator<SetRevisionRequiredStatusCommand> _validator;
    private readonly IVolunteerRequestsRepository _volunteerRequestsRepository;
    private readonly IPublisher _publisher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SetRevisionRequiredStatusHandler> _logger;

    public SetRevisionRequiredStatusHandler(
        IValidator<SetRevisionRequiredStatusCommand> validator,
        IVolunteerRequestsRepository volunteerRequestsRepository,
        IPublisher publisher,
        [FromKeyedServices(Modules.VolunteerRequests)] IUnitOfWork unitOfWork,
        ILogger<SetRevisionRequiredStatusHandler> logger)
    {
        _validator = validator;
        _volunteerRequestsRepository = volunteerRequestsRepository;
        _publisher = publisher;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        SetRevisionRequiredStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var existedRequest = await _volunteerRequestsRepository.GetById(
            command.RequestId, cancellationToken);
        if (existedRequest.IsFailure)
            return Errors.General.NotFound(command.RequestId).ToErrorList();
        
        if (existedRequest.Value.AdminId == null)
            return Errors.General.Failure("not on review").ToErrorList();
        
        if (existedRequest.Value.RejectionComment != null)
            return Errors.General.Failure("already rejected").ToErrorList();
        
        var rejectionComment = RejectionComment.Create(command.Comment).Value;
        existedRequest.Value.SetRevisionRequiredStatus(command.AdminId, rejectionComment);
        
        await _publisher.PublishDomainEvents(existedRequest.Value, cancellationToken);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Volunteer request with id {requestId} was rejected", command.RequestId);

        return existedRequest.Value.Id.Value;
    }
}