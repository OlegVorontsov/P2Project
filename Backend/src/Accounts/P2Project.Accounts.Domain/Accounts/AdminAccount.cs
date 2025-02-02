using P2Project.Accounts.Domain.Users.ValueObjects;
using P2Project.Volunteers.Domain.ValueObjects.Volunteers;

namespace P2Project.Accounts.Domain.Accounts;

public class AdminAccount
{
    private  AdminAccount(){}
    public const string ADMIN = "admin";
    public AdminAccount(User user)
    {
        Id = Guid.NewGuid();
        User = user;
        UserId = user.Id;
    }
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}