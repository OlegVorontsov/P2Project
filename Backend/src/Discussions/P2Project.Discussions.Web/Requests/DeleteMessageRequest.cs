using P2Project.Discussions.Application.DiscussionsManagement.Commands.DeleteMessage;

namespace P2Project.Discussions.Web.Requests;

public record DeleteMessageRequest(Guid MessageId)
{
    public DeleteMessageCommand ToCommand(Guid userId, Guid discussionId) =>
        new(userId, discussionId, MessageId);
}