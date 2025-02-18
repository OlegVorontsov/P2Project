using Microsoft.AspNetCore.Identity;
using P2Project.Accounts.Domain.Accounts;
using P2Project.Accounts.Domain.RolePermission.Roles;
using P2Project.Accounts.Domain.Users.ValueObjects;
using P2Project.SharedKernel.ValueObjects;
using SocialNetwork = P2Project.Accounts.Domain.Users.ValueObjects.SocialNetwork;

namespace P2Project.Accounts.Domain;

public class User : IdentityUser<Guid>
{
    private User(){}
    private List<Role> _roles = [];
    private List<Photo> _photos = [];
    
    public FullName FullName { get; set; } = null!;
    public IReadOnlyList<SocialNetwork>? SocialNetworks { get; private set; } = [];
    public IReadOnlyList<Role> Roles => _roles.AsReadOnly();
    public IReadOnlyList<Photo> Photos => _photos;
    public AdminAccount? AdminAccount { get; set; }
    public VolunteerAccount? VolunteerAccount { get; set; }
    public ParticipantAccount? ParticipantAccount { get; set; }

    public static User CreateAdmin(
        string email,
        string userName,
        FullName fullName,
        Role role)
    {
        return new User
        {
            Email = email,
            UserName = userName,
            FullName = fullName,
            _roles = [role]
        };
    }
    
    public static User CreateParticipant(
        string email,
        string userName,
        FullName fullName,
        Role role)
    {
        return new User
        {
            Email = email,
            UserName = userName,
            FullName = fullName,
            _roles = [role],
        };
    }
    
    public void AddVolunteerRole(Role role)
    {
        _roles.Add(role);
    }
}