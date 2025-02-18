using P2Project.Core.Interfaces.Commands;

namespace P2Project.Discussions.Application.DiscussionsManagement.Commands.Reopen;

public record ReopenCommand(
    Guid UserId, Guid DiscussionId, string Comment) : ICommand;