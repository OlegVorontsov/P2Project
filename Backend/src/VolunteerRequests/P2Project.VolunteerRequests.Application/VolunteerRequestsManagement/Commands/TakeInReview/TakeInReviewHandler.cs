using CSharpFunctionalExtensions;
using FluentValidation;
using MassTransit;
using MassTransit.DependencyInjection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Core;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.Discussions.Application.Interfaces;
using P2Project.SharedKernel.Errors;
using P2Project.VolunteerRequests.Agreements.Messages;
using P2Project.VolunteerRequests.Application.Interfaces;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.TakeInReview;

public class TakeInReviewHandler :
    ICommandHandler<Guid, TakeInReviewCommand>
{
    private readonly IValidator<TakeInReviewCommand> _validator;
    private readonly Bind<IDiscussionMessageBus, IPublishEndpoint> _publishEndpoint;
    private readonly IVolunteerRequestsRepository _volunteerRequestsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TakeInReviewHandler> _logger;

    public TakeInReviewHandler(
        IValidator<TakeInReviewCommand> validator,
        Bind<IDiscussionMessageBus,IPublishEndpoint> publishEndpoint,
        IVolunteerRequestsRepository volunteerRequestsRepository,
        [FromKeyedServices(Modules.VolunteerRequests)] IUnitOfWork unitOfWork,
        ILogger<TakeInReviewHandler> logger)
    {
        _validator = validator;
        _publishEndpoint = publishEndpoint;
        _volunteerRequestsRepository = volunteerRequestsRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        TakeInReviewCommand command,
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
        
        if (existedRequest.Value.AdminId != null)
            return Errors.General.Failure("already on review").ToErrorList();
        
        existedRequest.Value.TakeInReview(command.AdminId);
        
        await _publishEndpoint.Value.Publish(
            new VolunteerRequestReviewStartedEvent(
                existedRequest.Value.Id,
                command.AdminId,
                existedRequest.Value.UserId), cancellationToken);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Volunteer request {requestId} was taken on review", command.RequestId);

        return existedRequest.Value.Id.Value;
    }
}