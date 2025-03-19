using P2Project.SharedKernel;

namespace P2Project.Core.Events;

public record CreateDiscussionEvent(
    Guid ReviewingUserId, Guid ApplicantUserId) : IDomainEvent;