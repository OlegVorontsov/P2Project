namespace P2Project.Core.Dtos.Accounts;

public class ParticipantAccountDto
{
    public Guid ParticipantAccountId { get; set; }
    public Guid UserId { get; set; }
    public DateTime? BannedForRequestsUntil { get; set; }
}