using P2Project.SharedKernel;

namespace P2Project.Core.Events;

public record CreateDiscussionEvent(
    Guid RequestId,
    Guid ReviewingUserId,
    Guid ApplicantUserId) : IDomainEvent;