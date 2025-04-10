using P2Project.Accounts.Application.Commands.EmailManagement.ConfirmEmail;

namespace P2Project.Accounts.Web.Requests.EmailManagement;

public record ConfirmEmailRequest(Guid UserId, string Token)
{
    public static implicit operator ConfirmEmailCommand(ConfirmEmailRequest request)
        => new(request.UserId, request.Token);
}