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

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.TakeInReview;

public class TakeInReviewHandler(
    IValidator<TakeInReviewCommand> validator,
    IPublisher publisher,
    IVolunteerRequestsRepository volunteerRequestsRepository,
    [FromKeyedServices(Modules.VolunteerRequests)]
    IUnitOfWork unitOfWork,
    ILogger<TakeInReviewHandler> logger) : ICommandHandler<Guid, TakeInReviewCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        TakeInReviewCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(
            command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var existedRequest = await volunteerRequestsRepository.GetById(
            command.RequestId, cancellationToken);
        if (existedRequest.IsFailure)
            return Errors.General.NotFound(command.RequestId).ToErrorList();
        
        if (existedRequest.Value.AdminId != null)
            return Errors.General.Failure("already on review").ToErrorList();
        
        existedRequest.Value.TakeInReview(command.AdminId);
        
        await publisher.PublishDomainEvents(existedRequest.Value, cancellationToken);
        
        await unitOfWork.SaveChanges(cancellationToken);
        
        logger.LogInformation("Volunteer request {requestId} was taken on review", command.RequestId);

        return existedRequest.Value.Id.Value;
    }
}