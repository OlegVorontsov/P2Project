using P2Project.Accounts.Domain;

namespace P2Project.Accounts.Application;

public interface ITokenProvider
{
    public string GenerateAccessToken(User user);
}