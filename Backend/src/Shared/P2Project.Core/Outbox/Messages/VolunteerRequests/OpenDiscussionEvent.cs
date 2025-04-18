namespace P2Project.Core.Outbox.Messages.VolunteerRequests;

public record OpenDiscussionEvent(
    Guid RequesterId,
    Guid ReviewingUserId,
    Guid ApplicantUserId,
    string UserName);