namespace P2Project.Core.Outbox.Messages.VolunteerRequests;

public record CreateVolunteerAccountEvent(
    Guid UserId,
    string UserName,
    int Age,
    int Grade,
    string Gender);