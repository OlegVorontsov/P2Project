using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Core;
using P2Project.Core.Events;
using P2Project.Core.Extensions;
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
    private readonly ILogger<CreateMessageHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMessageHandler(
        IValidator<CreateMessageEvent> validator,
        IDiscussionsRepository discussionsRepository,
        ILogger<CreateMessageHandler> logger,
        [FromKeyedServices(Modules.Discussions)] IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _discussionsRepository = discussionsRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
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
        //from here
        var newMessage = Message.Create(
            discussionExist.Value.Id,
            command.SenderId,
            Content.Create(command.Message).Value);

        discussionExist.Value.AddMessage(newMessage);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation(
            "Message was created in discussion with id {discussionId}",
            discussionExist.Value.Id);
        
        return newMessage;
    }
}