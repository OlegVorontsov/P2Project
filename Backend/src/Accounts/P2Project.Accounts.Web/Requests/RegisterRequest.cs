using P2Project.Accounts.Application.Commands;

namespace P2Project.Accounts.Web.Requests;

public record RegisterRequest(
    string Email, string UserName, string Password)
{
    public RegisterCommand ToCommand() =>
        new (Email, UserName, Password);
}