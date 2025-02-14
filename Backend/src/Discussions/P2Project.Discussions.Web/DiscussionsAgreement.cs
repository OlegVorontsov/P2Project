using CSharpFunctionalExtensions;
using P2Project.Discussions.Agreements;
using P2Project.Discussions.Application.DiscussionsManagement.Commands.Create;
using P2Project.Discussions.Application.DiscussionsManagement.Commands.CreateMessage;
using P2Project.SharedKernel.Errors;

namespace P2Project.Discussions.Web;

public class DiscussionsAgreement : IDiscussionsAgreement
{
    private readonly CreateDiscussionHandler _createDiscussionHandler;
    private readonly CreateMessageHandler _createMessageHandler;

    public DiscussionsAgreement(
        CreateDiscussionHandler createDiscussionHandler,
        CreateMessageHandler createMessageHandler)
    {
        _createDiscussionHandler = createDiscussionHandler;
        _createMessageHandler = createMessageHandler;
    }
    public async Task<Result<Guid, ErrorList>> CreateDiscussion(
        Guid reviewingUserId,
        Guid applicantUserId,
        CancellationToken cancellationToken = default)
    {
        var createCommand = new CreateDiscussionCommand(reviewingUserId, applicantUserId);
        
        var discussion = await _createDiscussionHandler.Handle(createCommand, cancellationToken);
        if (discussion.IsFailure)
            return discussion.Error;
        
        return discussion.Value.DiscussionId;
    }

    public async Task<Result<Guid, ErrorList>> CreateMessage(
        Guid senderId,
        string message,
        CancellationToken cancellationToken = default)
    {
        var createMessageCommand = new CreateMessageCommand(senderId, message);
        
        var messageCreate = await _createMessageHandler.Handle(
            createMessageCommand, cancellationToken);
        if (messageCreate.IsFailure)
            return messageCreate.Error;
        
        return messageCreate.Value.Id;
    }
}