using P2Project.Discussions.Application.DiscussionsManagement.Commands.Reopen;

namespace P2Project.Discussions.Web.Requests;

public record ReopenRequest(string Comment)
{
    public ReopenCommand ToCommand(Guid userId, Guid discussionId) =>
        new(userId, discussionId, Comment);
}