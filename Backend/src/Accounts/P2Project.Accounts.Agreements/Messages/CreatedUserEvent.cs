namespace P2Project.Accounts.Agreements.Messages;

public record CreatedUserEvent(
    Guid UserId,
    string TelegramUserId,
    string Email,
    string UserName,
    string RoleName);