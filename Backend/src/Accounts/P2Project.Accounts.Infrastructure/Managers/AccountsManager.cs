using P2Project.Accounts.Application.Interfaces;
using P2Project.Accounts.Domain.Accounts;
using P2Project.Accounts.Infrastructure.DbContexts;

namespace P2Project.Accounts.Infrastructure.Managers;

public class AccountsManager(AuthorizationDbContext dbContext) : IAccountsManager
{
    public async Task CreateAdminAccount(
        AdminAccount adminAccount, CancellationToken cancellationToken)
    {
        dbContext.AdminAccounts.Add(adminAccount);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task CreateVolunteerAccount(
        VolunteerAccount volunteerAccount, CancellationToken cancellationToken)
    {
        dbContext.VolunteerAccounts.Add(volunteerAccount);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task CreateParticipantAccount(
        ParticipantAccount participantAccount, CancellationToken cancellationToken)
    {
        dbContext.ParticipantAccounts.Add(participantAccount);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}