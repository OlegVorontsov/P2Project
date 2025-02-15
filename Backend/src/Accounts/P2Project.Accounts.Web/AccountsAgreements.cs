using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Agreements;
using P2Project.Accounts.Infrastructure.DbContexts;
using P2Project.Core;
using P2Project.Core.Interfaces;

namespace P2Project.Accounts.Web;

public class AccountsAgreements : IAccountsAgreements
{
    private readonly AccountsWriteDbContext _accountsWriteDbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AccountsAgreements> _logger;

    public AccountsAgreements(
        AccountsWriteDbContext accountsWriteDbContext,
        [FromKeyedServices(Modules.Accounts)] IUnitOfWork unitOfWork,
        ILogger<AccountsAgreements> logger)
    {
        _accountsWriteDbContext = accountsWriteDbContext;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task BanUser(Guid userId, CancellationToken cancellationToken)
    {
        var participantAccount = await _accountsWriteDbContext.ParticipantAccounts
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        var until = DateTime.UtcNow.AddDays(7);
        if (participantAccount != null)
            participantAccount.BanForRequestsForWeek(until);

        await _unitOfWork.SaveChanges(cancellationToken);
        _logger.LogInformation("User {userId} banned for requests until {until}",
            userId, until);
    }
}