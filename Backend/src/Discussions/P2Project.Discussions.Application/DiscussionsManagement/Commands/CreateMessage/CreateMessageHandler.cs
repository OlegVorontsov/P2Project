using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Core;
using P2Project.Core.Events;
using P2Project.Core.Interfaces;
using P2Project.Discussions.Application.Interfaces;
using P2Project.Discussions.Domain.Entities;
using P2Project.SharedKernel.ValueObjects;

namespace P2Project.Discussions.Application.DiscussionsManagement.Commands.CreateMessage;

public class CreateMessageHandler :
    INotificationHandler<CreateMessageEvent>
{
    private readonly IValidator<CreateMessageEvent> _validator;
    private readonly IDiscussionsRepository _discussionsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateMessageHandler> _logger;

    public CreateMessageHandler(
        IValidator<CreateMessageEvent> validator,
        IDiscussionsRepository discussionsRepository,
        [FromKeyedServices(Modules.Discussions)] IUnitOfWork unitOfWork,
        ILogger<CreateMessageHandler> logger)
    {
        _validator = validator;
        _discussionsRepository = discussionsRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(
        CreateMessageEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(
            domainEvent, cancellationToken);
        if (validationResult.IsValid == false)
            throw new Exception("Create message validation failed");
        
        var discussionExist = await _discussionsRepository.GetByRequestId(
            domainEvent.RequestId, cancellationToken);
        if (discussionExist.IsFailure)
            throw new Exception(discussionExist.Error.Message);

        var newMessage = Message.Create(
            discussionExist.Value.Id,
            domainEvent.SenderId,
            Content.Create(domainEvent.Message).Value);

        var addMessageResult = discussionExist.Value.AddMessage(newMessage);
        if (addMessageResult.IsFailure)
            throw new Exception(addMessageResult.Error.Message);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation(
            "Message was created in discussion with id {Id}",
            discussionExist.Value.Id);
    }
}