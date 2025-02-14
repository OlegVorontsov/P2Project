namespace P2Project.Accounts.Agreements;

public interface IAdminAccountsAgreement
{
    Task<bool> IsAnyAdminAccountByUserId(
        Guid userId, CancellationToken cancellationToken = default);
}