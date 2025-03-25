using P2Project.SharedKernel;

namespace P2Project.Core.Events;

public record ApprovedEvent(Guid UserId) : IDomainEvent;