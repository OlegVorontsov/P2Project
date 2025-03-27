using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Core;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.Discussions.Application.Interfaces;
using P2Project.Discussions.Domain.Entities;
using P2Project.SharedKernel.ValueObjects;

namespace P2Project.Discussions.Application.DiscussionsManagement.EventHandlers.CreateMessage;

public class CreateMessageHandler(
    IDiscussionsRepository discussionsRepository,
    [FromKeyedServices(Modules.Discussions)] IUnitOfWork unitOfWork,
    ILogger<CreateMessageHandler> logger) : ICommandVoidHandler<CreateMessageCommand>
{
    public async Task Handle(
        CreateMessageCommand command,
        CancellationToken cancellationToken)
    {
        var discussionExist = await discussionsRepository.GetByRequestId(
            command.RequestId, cancellationToken);
        if (discussionExist.IsFailure)
            throw new Exception(discussionExist.Error.Message);

        var newMessage = Message.Create(
            discussionExist.Value.Id,
            command.SenderId,
            Content.Create(command.Message).Value);

        var addMessageResult = discussionExist.Value.AddMessage(newMessage);
        if (addMessageResult.IsFailure)
            throw new Exception(addMessageResult.Error.Message);
        
        await unitOfWork.SaveChanges(cancellationToken);
        
        logger.LogInformation(
            "Message was created in discussion with id {Id}",
            discussionExist.Value.Id);
    }
}