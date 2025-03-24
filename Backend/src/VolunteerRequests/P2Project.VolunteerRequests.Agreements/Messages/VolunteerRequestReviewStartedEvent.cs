namespace P2Project.VolunteerRequests.Agreements.Messages;

public record VolunteerRequestReviewStartedEvent(
    Guid RequesterId,
    Guid ReviewingUserId,
    Guid ApplicantUserId);