using Microsoft.AspNetCore.Identity;
using P2Project.Accounts.Domain.RolePermission.Roles;
using SocialNetwork = P2Project.Accounts.Domain.Users.ValueObjects.SocialNetwork;

namespace P2Project.Accounts.Domain;

public class User : IdentityUser<Guid>
{
    private User(){}
    private List<Role> _roles = [];
    public IReadOnlyList<SocialNetwork>? SocialNetworks { get; private set; } = [];
    public IReadOnlyList<Role> Roles => _roles;

    public static User CreateAdmin(string userName, string email, Role role)
    {
        return new User
        {
            UserName = userName,
            Email = email,
            _roles = [role]
        };
    }
}