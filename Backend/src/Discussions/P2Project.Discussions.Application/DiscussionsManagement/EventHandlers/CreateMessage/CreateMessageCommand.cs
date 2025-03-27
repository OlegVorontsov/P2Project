using P2Project.Core.Interfaces.Commands;

namespace P2Project.Discussions.Application.DiscussionsManagement.EventHandlers.CreateMessage;

public record CreateMessageCommand(
    Guid RequestId, Guid SenderId, string Message) : ICommand;
