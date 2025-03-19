using CSharpFunctionalExtensions;
using P2Project.Discussions.Agreements;
using P2Project.Discussions.Application.DiscussionsManagement.Commands.CreateMessage;
using P2Project.SharedKernel.Errors;

namespace P2Project.Discussions.Web;

public class DiscussionsAgreement : IDiscussionsAgreement
{
    private readonly CreateMessageHandler _createMessageHandler;

    public DiscussionsAgreement(
        CreateMessageHandler createMessageHandler)
    {
        _createMessageHandler = createMessageHandler;
    }

    public async Task<Result<Guid, ErrorList>> CreateMessage(
        Guid senderId, Guid participantId,
        string message,
        CancellationToken cancellationToken = default)
    {
        var createMessageCommand = new CreateMessageCommand(
            senderId, participantId, message);
        
        var messageCreate = await _createMessageHandler.Handle(
            createMessageCommand, cancellationToken);
        if (messageCreate.IsFailure)
            return messageCreate.Error;
        
        return messageCreate.Value.Id;
    }
}