namespace P2Project.Accounts.Domain.Accounts;

public class ParticipantAccount
{
    public const string RoleName = "participant";
    private ParticipantAccount(){}
    public ParticipantAccount(User user)
    {
        Id = Guid.NewGuid();
        User = user;
        UserId = user.Id;
    }
    
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public DateTime? BannedForRequestsUntil { get; set; }

    public void BanForRequestsForWeek(DateTime date) =>
        BannedForRequestsUntil = date;
    
    public void UnbanForRequests() => BannedForRequestsUntil = null;
}