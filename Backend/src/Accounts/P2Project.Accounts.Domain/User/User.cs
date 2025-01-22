using Microsoft.AspNetCore.Identity;
using P2Project.Accounts.Domain.User.ValueObjects;
using P2Project.SharedKernel.ValueObjects;

namespace P2Project.Accounts.Domain.User;

public class User : IdentityUser<Guid>
{
    public IReadOnlyList<SocialNetwork>? SocialNetworks { get; private set; } = [];
}