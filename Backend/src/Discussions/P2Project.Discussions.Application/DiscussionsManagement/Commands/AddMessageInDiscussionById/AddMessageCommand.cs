using P2Project.Core.Interfaces.Commands;

namespace P2Project.Discussions.Application.DiscussionsManagement.Commands.AddMessageInDiscussionById;

public record AddMessageCommand(
    Guid SenderId, Guid DiscussionId, string Message) : ICommand;