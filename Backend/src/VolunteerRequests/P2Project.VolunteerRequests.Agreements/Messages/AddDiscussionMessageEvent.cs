namespace P2Project.VolunteerRequests.Agreements.Messages;

public record AddDiscussionMessageEvent(
    Guid RequesterId,
    Guid SenderId,
    string Message);