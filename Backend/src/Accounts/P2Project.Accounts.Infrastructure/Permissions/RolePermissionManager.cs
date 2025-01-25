using Microsoft.EntityFrameworkCore;
using P2Project.Accounts.Domain.RolePermission;
using P2Project.Accounts.Infrastructure.DbContexts;

namespace P2Project.Accounts.Infrastructure.Permissions;

public class RolePermissionManager(AuthorizationDbContext dbContext)
{
    public async Task AddRangeIfDoesNotExist(Guid roleId, IEnumerable<string> permissionCodes)
    {
        foreach (var code in permissionCodes)
        {
            var permission = await dbContext.Permissions.FirstOrDefaultAsync(p => p.Code == code);
            if (permission is null) throw new Exception($"Permission {code} is not found");
                
            var rolePermissionExist = await dbContext.RolePermissions
                .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission.Id);
            if(rolePermissionExist) continue;

            dbContext.RolePermissions.Add(new RolePermission
            {
                RoleId = roleId,
                PermissionId = permission!.Id
            });
        }
        await dbContext.SaveChangesAsync();
    }
}