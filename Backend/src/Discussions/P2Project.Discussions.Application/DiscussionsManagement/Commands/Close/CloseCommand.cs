using P2Project.Core.Interfaces.Commands;

namespace P2Project.Discussions.Application.DiscussionsManagement.Commands.Close;

public record CloseCommand(Guid UserId, Guid DiscussionId, string Comment) : ICommand;