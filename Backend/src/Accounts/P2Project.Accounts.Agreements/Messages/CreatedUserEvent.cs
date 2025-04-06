namespace P2Project.Accounts.Agreements.Messages;

public record CreatedUserEvent(Guid UserId, string Email, string UserName, string Role);