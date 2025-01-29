using P2Project.Accounts.Domain;

namespace P2Project.Accounts.Application.Interfaces;

public interface ITokenProvider
{
    public Task<string> GenerateAccessToken(User user);
}