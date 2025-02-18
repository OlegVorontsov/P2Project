using P2Project.Discussions.Application.DiscussionsManagement.Commands.Close;

namespace P2Project.Discussions.Web.Requests;

public record CloseRequest(string Comment)
{
    public CloseCommand ToCommand(Guid userId, Guid discussionId) =>
        new(userId, discussionId, Comment);
}