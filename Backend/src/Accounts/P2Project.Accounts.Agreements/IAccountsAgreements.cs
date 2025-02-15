namespace P2Project.Accounts.Agreements;

public interface IAccountsAgreements
{
    public Task BanUser(Guid userId, CancellationToken cancellationToken);
}