using P2Project.Volunteers.Domain.ValueObjects.Volunteers;

namespace P2Project.Accounts.Domain.Accounts;

public class AdminAccount
{
    private  AdminAccount(){}
    public const string ADMIN = "Admin";
    public AdminAccount(FullName fullName, User user)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
    }
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public FullName FullName { get; set; }
}