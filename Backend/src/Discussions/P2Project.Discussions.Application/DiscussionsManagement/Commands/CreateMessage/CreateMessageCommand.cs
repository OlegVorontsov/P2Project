using P2Project.Core.Interfaces.Commands;

namespace P2Project.Discussions.Application.DiscussionsManagement.Commands.CreateMessage;

public record CreateMessageCommand(
    Guid SenderId, string Message) : ICommand;