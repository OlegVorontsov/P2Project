using P2Project.Discussions.Application.DiscussionsManagement.Commands.AddMessageInDiscussionById;

namespace P2Project.Discussions.Web.Requests;

public record AddMessageRequest(string Comment)
{
    public AddMessageCommand ToCommand(Guid userId, Guid discussionId) =>
        new(userId, discussionId, Comment);
}