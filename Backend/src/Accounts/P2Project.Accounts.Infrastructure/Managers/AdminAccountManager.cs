using P2Project.Accounts.Domain.Accounts;
using P2Project.Accounts.Infrastructure.DbContexts;

namespace P2Project.Accounts.Infrastructure.Managers;

public class AdminAccountManager(AuthorizationDbContext dbContext)
{
    public async Task CreateAdminAccount(AdminAccount adminAccount)
    {
        await dbContext.AdminAccounts.AddAsync(adminAccount);
        await dbContext.SaveChangesAsync();
    }
}