using P2Project.Core.Interfaces.Commands;

namespace NotificationService.Application.EventHandlers;

public record ConfirmationEmailCommand(Guid UserId) : ICommand;