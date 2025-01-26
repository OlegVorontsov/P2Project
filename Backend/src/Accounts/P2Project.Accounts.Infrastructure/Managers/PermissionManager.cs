using Microsoft.EntityFrameworkCore;
using P2Project.Accounts.Domain.RolePermission.Permissions;
using P2Project.Accounts.Infrastructure.DbContexts;

namespace P2Project.Accounts.Infrastructure.Managers;

public class PermissionManager(AuthorizationDbContext dbContext)
{
    public async Task<Permission?> FindByCode(string code) =>
        await dbContext.Permissions.FirstOrDefaultAsync(p => p.Code == code);
    
    public async Task AddRangeIfDoesNotExist(IEnumerable<string> codes)
    {
        foreach (var code in codes)
        {
            var permissionExist = await dbContext.Permissions.AnyAsync(p => p.Code == code);
            if(permissionExist) continue;
            await dbContext.Permissions.AddAsync(new Permission { Code = code });
        }
        await dbContext.SaveChangesAsync();
    }
}