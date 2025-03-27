using P2Project.Core.Interfaces.Commands;

namespace P2Project.Accounts.Application.EventHandlers.CreateVolunteerAccount;

public record CreateVolunteerAccountCommand(Guid UserId) : ICommand;