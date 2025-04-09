namespace P2Project.Accounts.Agreements.Messages;

public record ConfirmedUserEmailEvent(
    Guid UserId,
    string Email,
    string UserName,
    string EmailConfirmationLink);