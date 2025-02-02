using Microsoft.EntityFrameworkCore;
using P2Project.Accounts.Domain.RolePermission.Permissions;
using P2Project.Accounts.Infrastructure.DbContexts;

namespace P2Project.Accounts.Infrastructure.Managers;

public class PermissionManager(AuthorizationDbContext dbContext)
{
    public async Task<Permission?> FindByCode(string code) =>
        await dbContext.Permissions.FirstOrDefaultAsync(p => p.Code == code);
    
    public async Task AddRangeIfDoesNotExist(
        IEnumerable<string> codes, CancellationToken cancellationToken = default)
    {
        foreach (var code in codes)
        {
            var permissionExist = await dbContext.Permissions
                .AnyAsync(p => p.Code == code, cancellationToken);
            if(permissionExist) continue;
            await dbContext.Permissions.AddAsync(
                new Permission { Code = code }, cancellationToken);
        }
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<HashSet<string>> GetUserPermissions(
        Guid userId, CancellationToken cancellationToken = default)
    {
        var permissions = await dbContext.Users
            .Include(u => u.Roles)
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Roles)
            .SelectMany(r => r.RolePermissions)
            .Select(rp => rp.Permission.Code)
            .ToListAsync(cancellationToken);
        
        return permissions.ToHashSet();
    }
}