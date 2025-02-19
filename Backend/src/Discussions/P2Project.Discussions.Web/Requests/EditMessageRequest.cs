using P2Project.Discussions.Application.DiscussionsManagement.Commands.EditMessage;

namespace P2Project.Discussions.Web.Requests;

public record EditMessageRequest(Guid MessageId, string MessageText)
{
    public EditMessageCommand ToCommand(Guid userId, Guid discussionId) =>
        new(userId, discussionId, MessageId, MessageText);
}