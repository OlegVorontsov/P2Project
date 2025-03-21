using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Agreements;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Accounts.Domain.RolePermission.Roles;
using P2Project.Accounts.Infrastructure.DbContexts;
using P2Project.Accounts.Infrastructure.Managers;
using P2Project.Core;
using P2Project.Core.Interfaces;

namespace P2Project.Accounts.Web;

public class AccountsAgreements : IAccountsAgreements
{
    private readonly PermissionManager _permissionManager;
    private readonly AccountsWriteDbContext _accountsWriteDbContext;
    private readonly IAccountsReadDbContext _accountsReadDbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AccountsAgreements> _logger;

    public AccountsAgreements(
        PermissionManager permissionManager,
        AccountsWriteDbContext accountsWriteDbContext,
        IAccountsReadDbContext accountsReadDbContext,
        [FromKeyedServices(Modules.Accounts)] IUnitOfWork unitOfWork,
        ILogger<AccountsAgreements> logger)
    {
        _permissionManager = permissionManager;
        _accountsWriteDbContext = accountsWriteDbContext;
        _accountsReadDbContext = accountsReadDbContext;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<HashSet<string>> GetUserPermissionCodes(Guid userId)
    {
        return await _permissionManager.GetUserPermissions(userId);
    }

    public async Task<HashSet<Role>> GetUserRoles(Guid userId)
    {
        return await _permissionManager.GetUserRoles(userId);
    }
    
    public async Task<bool> IsUserBannedForVolunteerRequests(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var userDto = await _accountsReadDbContext.ParticipantAccounts
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        return DateTime.UtcNow < userDto!.BannedForRequestsUntil;
    }

    public async Task BanUser(Guid userId, CancellationToken cancellationToken)
    {
        var participantAccount = await _accountsWriteDbContext.ParticipantAccounts
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        var until = DateTime.UtcNow.AddDays(7);
        
        if (participantAccount == null) return;
        
        participantAccount.BanForRequestsForWeek(until);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("User {userId} banned for requests until {until}",
            userId, until);
    }
    
    public async Task UnbanUser(Guid userId, CancellationToken cancellationToken)
    {
        var participantAccount = await _accountsWriteDbContext.ParticipantAccounts
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        
        if (participantAccount == null) return;
        
        participantAccount.UnbanForRequests();
    }
}