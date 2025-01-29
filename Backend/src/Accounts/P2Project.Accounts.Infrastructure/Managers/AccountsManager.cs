using P2Project.Accounts.Application.Interfaces;
using P2Project.Accounts.Domain.Accounts;
using P2Project.Accounts.Infrastructure.DbContexts;

namespace P2Project.Accounts.Infrastructure.Managers;

public class AccountsManager(AuthorizationDbContext dbContext) : IAccountsManager
{
    public async Task CreateAdminAccount(AdminAccount adminAccount)
    {
        await dbContext.AdminAccounts.AddAsync(adminAccount);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task CreateVolunteerAccount(VolunteerAccount volunteerAccount)
    {
        await dbContext.VolunteerAccounts.AddAsync(volunteerAccount);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task CreateParticipantAccount(ParticipantAccount participantAccount)
    {
        await dbContext.ParticipantAccounts.AddAsync(participantAccount);
        await dbContext.SaveChangesAsync();
    }
}