using P2Project.SharedKernel;

namespace P2Project.Core.Events;

public record CreateVolunteerAccountEvent(Guid UserId) : IDomainEvent;