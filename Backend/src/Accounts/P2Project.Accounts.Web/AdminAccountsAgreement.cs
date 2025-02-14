using P2Project.Accounts.Agreements;
using P2Project.Accounts.Application.Queries.IsAnyAdminAccountByUserId;

namespace P2Project.Accounts.Web;

public class AdminAccountsAgreement : IAdminAccountsAgreement
{
    private readonly IsAnyAdminAccountByUserIdHandler _handler;

    public AdminAccountsAgreement(IsAnyAdminAccountByUserIdHandler handler)
    {
        _handler = handler;
    }

    public async Task<bool> IsAnyAdminAccountByUserId(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var query = new IsAnyAdminAccountByUserIdQuery(userId);
        return await _handler.Handle(query, cancellationToken);
    }
}