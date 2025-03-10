using CSharpFunctionalExtensions;
using FilesService.Core.Models;
using Microsoft.AspNetCore.Identity;
using P2Project.Accounts.Domain.Accounts;
using P2Project.Accounts.Domain.RolePermission.Roles;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.ValueObjects;
using SocialNetwork = P2Project.Accounts.Domain.Users.ValueObjects.SocialNetwork;

namespace P2Project.Accounts.Domain;

public class User : IdentityUser<Guid>
{
    private User(){}
    private List<Role> _roles = [];
    
    public FullName FullName { get; set; } = null!;
    public IReadOnlyList<SocialNetwork>? SocialNetworks { get; private set; } = [];
    public IReadOnlyList<Role> Roles => _roles.AsReadOnly();
    public AdminAccount? AdminAccount { get; set; }
    public VolunteerAccount? VolunteerAccount { get; set; }
    public ParticipantAccount? ParticipantAccount { get; set; }
    public IReadOnlyList<MediaFile> Photos { get; private set; } = [];
    public IReadOnlyList<string> PhotosUrls { get; set; } = [];
    public MediaFile? Avatar { get; private set; }
    public string AvatarUrl { get; set; } = string.Empty;

    private User(
        string email,
        string userName,
        FullName fullName,
        Role role,
        MediaFile avatar = null)
    {
        Email = email;
        UserName = userName;
        FullName = fullName;
        _roles = [role];
        Avatar = avatar;
    }
    
    public static Result<User, Error> Create(
        string email,
        string userName,
        FullName fullName,
        Role role,
        MediaFile avatar = null)
    {
        if (email != null && userName != null && fullName != null && role != null)
            return new User(email, userName, fullName, role, avatar);

        return Errors.General.ValueIsInvalid("User");
    }
    
    public void AddRole(Role role)
    {
        _roles.Add(role);
    }
}