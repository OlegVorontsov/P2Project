namespace P2Project.Accounts.Agreements;

public interface IAccountsAgreements
{
    public Task BanUser(Guid userId, CancellationToken cancellationToken);

    public Task<bool> IsUserBannedForVolunteerRequests(
        Guid userId, CancellationToken cancellationToken);
}