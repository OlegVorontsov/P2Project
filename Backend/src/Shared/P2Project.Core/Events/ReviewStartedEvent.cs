using P2Project.SharedKernel;

namespace P2Project.Core.Events;

public record ReviewStartedEvent(
    Guid RequestId,
    Guid AdminId,
    Guid UserId) : IDomainEvent;