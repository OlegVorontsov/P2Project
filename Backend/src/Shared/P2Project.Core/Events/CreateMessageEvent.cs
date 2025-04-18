using P2Project.SharedKernel;

namespace P2Project.Core.Events;

public record CreateMessageEvent(
    Guid RequestId,
    Guid SenderId,
    Guid RequestUserId,
    string UserName,
    string Message) : IDomainEvent;