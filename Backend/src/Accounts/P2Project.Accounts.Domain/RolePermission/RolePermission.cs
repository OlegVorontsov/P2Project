using P2Project.Accounts.Domain.RolePermission.Permissions;
using P2Project.Accounts.Domain.RolePermission.Roles;

namespace P2Project.Accounts.Domain.RolePermission;

public class RolePermission
{
    public Guid RoleId { get; set; }
    public Role Role { get; set; }
    
    public Guid PermissionId { get; set; }
    public Permission Permission { get; set; }
}