using P2Project.Core.Interfaces.Commands;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.SetReopenStatus;

public record SetReopenStatusCommand(
    Guid UserId, Guid RequestId, string Comment) : ICommand;