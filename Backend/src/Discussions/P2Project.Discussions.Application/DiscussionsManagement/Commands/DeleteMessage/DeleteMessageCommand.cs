using P2Project.Core.Interfaces.Commands;

namespace P2Project.Discussions.Application.DiscussionsManagement.Commands.DeleteMessage;

public record DeleteMessageCommand(
    Guid SenderId, Guid DiscussionId, Guid MessageId) : ICommand;