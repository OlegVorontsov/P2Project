using P2Project.Core.Interfaces.Commands;

namespace P2Project.Discussions.Application.DiscussionsManagement.Commands.EditMessage;

public record EditMessageCommand(
    Guid SenderId, Guid DiscussionId, Guid MessageId, string MessageText) : ICommand;