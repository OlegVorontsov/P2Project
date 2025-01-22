using Microsoft.AspNetCore.Identity;

namespace P2Project.Accounts.Domain.RolePermission.Roles;

public class Role : IdentityRole<Guid>
{
    public IReadOnlyList<RolePermission> RolePermissions { get; set; }
}