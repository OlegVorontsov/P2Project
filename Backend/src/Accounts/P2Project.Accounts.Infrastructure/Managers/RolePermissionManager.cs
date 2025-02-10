using Microsoft.EntityFrameworkCore;
using P2Project.Accounts.Domain.RolePermission;
using P2Project.Accounts.Infrastructure.DbContexts;

namespace P2Project.Accounts.Infrastructure.Managers;

public class RolePermissionManager(AccountsWriteDbContext writeDbContext)
{
    public async Task AddRangeIfDoesNotExist(
        Guid roleId,
        IEnumerable<string> permissionCodes,
        CancellationToken cancellationToken = default)
    {
        foreach (var code in permissionCodes)
        {
            var permission = await writeDbContext.Permissions.FirstOrDefaultAsync(p => p.Code == code);
            if (permission is null) throw new Exception($"Permission {code} is not found");
                
            var rolePermissionExist = await writeDbContext.RolePermissions
                .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission.Id, cancellationToken);
            if(rolePermissionExist) continue;

            writeDbContext.RolePermissions.Add(new RolePermission
            {
                RoleId = roleId,
                PermissionId = permission!.Id
            });
        }
        await writeDbContext.SaveChangesAsync(cancellationToken);
    }
}