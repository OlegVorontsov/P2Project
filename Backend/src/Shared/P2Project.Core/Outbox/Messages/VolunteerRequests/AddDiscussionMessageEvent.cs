namespace P2Project.Core.Outbox.Messages.VolunteerRequests;

public record AddDiscussionMessageEvent(
    Guid RequesterId,
    Guid SenderId,
    Guid RequestUserId,
    string UserName,
    string Message);