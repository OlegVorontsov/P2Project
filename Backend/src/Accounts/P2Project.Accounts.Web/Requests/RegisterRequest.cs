using P2Project.Accounts.Application.Commands;
using P2Project.Accounts.Application.Commands.Register;
using P2Project.Core.Dtos.Volunteers;

namespace P2Project.Accounts.Web.Requests;

public record RegisterRequest(FullNameDto FullName,
    string Email, string UserName, string Password)
{
    public RegisterCommand ToCommand() =>
        new (FullName, Email, UserName, Password);
}