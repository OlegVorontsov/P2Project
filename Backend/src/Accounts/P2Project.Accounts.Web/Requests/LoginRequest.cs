using P2Project.Accounts.Application.Commands;
using P2Project.Accounts.Application.Commands.Login;
using P2Project.Accounts.Application.Commands.Register;

namespace P2Project.Accounts.Web.Requests;

public record LoginRequest(
    string Email, string Password)
{
    public LoginCommand ToCommand() =>
        new (Email, Password);
}