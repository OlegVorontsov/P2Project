using P2Project.Accounts.Application.Commands.EmailManagement.GenerateEmailConfirmationToken;

namespace P2Project.Accounts.Web.Requests.EmailManagement;

public record GenerateEmailConfirmationTokenRequest(Guid UserId)
{
    public static implicit operator GenerateEmailConfirmationTokenCommand(GenerateEmailConfirmationTokenRequest request)
        => new(request.UserId);
}