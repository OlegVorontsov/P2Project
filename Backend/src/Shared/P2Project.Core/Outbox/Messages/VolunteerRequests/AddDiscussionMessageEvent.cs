namespace P2Project.Core.Outbox.Messages.VolunteerRequests;

public record AddDiscussionMessageEvent(
    Guid RequesterId,
    Guid SenderId,
    string Message);