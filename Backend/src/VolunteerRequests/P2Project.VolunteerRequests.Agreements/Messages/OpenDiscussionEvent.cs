namespace P2Project.VolunteerRequests.Agreements.Messages;

public record OpenDiscussionEvent(
    Guid RequesterId,
    Guid ReviewingUserId,
    Guid ApplicantUserId);