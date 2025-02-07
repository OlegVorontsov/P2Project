using Microsoft.EntityFrameworkCore;
using P2Project.Accounts.Domain.RolePermission.Permissions;
using P2Project.Accounts.Infrastructure.DbContexts;

namespace P2Project.Accounts.Infrastructure.Managers;

public class PermissionManager(AccountsWriteDbContext writeDbContext)
{
    public async Task<Permission?> FindByCode(string code) =>
        await writeDbContext.Permissions.FirstOrDefaultAsync(p => p.Code == code);
    
    public async Task AddRangeIfDoesNotExist(
        IEnumerable<string> codes, CancellationToken cancellationToken = default)
    {
        foreach (var code in codes)
        {
            var permissionExist = await writeDbContext.Permissions
                .AnyAsync(p => p.Code == code, cancellationToken);
            if(permissionExist) continue;
            await writeDbContext.Permissions.AddAsync(
                new Permission { Code = code }, cancellationToken);
        }
        await writeDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<HashSet<string>> GetUserPermissions(
        Guid userId, CancellationToken cancellationToken = default)
    {
        var permissions = await writeDbContext.Users
            .Include(u => u.Roles)
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Roles)
            .SelectMany(r => r.RolePermissions)
            .Select(rp => rp.Permission.Code)
            .ToListAsync(cancellationToken);
        
        return permissions.ToHashSet();
    }
}