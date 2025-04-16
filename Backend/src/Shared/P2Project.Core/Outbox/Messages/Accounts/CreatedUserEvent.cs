namespace P2Project.Core.Outbox.Messages.Accounts;

public record CreatedUserEvent(
    Guid UserId,
    string TelegramUserId,
    string Email,
    string UserName,
    string RoleName);