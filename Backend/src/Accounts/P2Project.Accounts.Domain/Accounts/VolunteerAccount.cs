using P2Project.SharedKernel.ValueObjects;

namespace P2Project.Accounts.Domain.Accounts;

public class VolunteerAccount
{
    public const string RoleName = "Volunteer";
    public VolunteerAccount(){}
    public VolunteerAccount(User user, Experience experience)
    {
        Id = Guid.NewGuid();
        User = user;
        UserId = user.Id;
        Experience = experience;
    }
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Experience Experience { get; private set; } = default!;
    public IReadOnlyList<AssistanceDetail>? AssistanceDetails { get; private set; }
    public IReadOnlyList<Certificate>? Certificates { get; private set; }
}