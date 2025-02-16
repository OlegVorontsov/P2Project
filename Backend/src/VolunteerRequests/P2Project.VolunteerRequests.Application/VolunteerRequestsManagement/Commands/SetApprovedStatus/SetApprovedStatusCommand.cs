using P2Project.Core.Interfaces.Commands;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.SetApprovedStatus;

public record SetApprovedStatusCommand(
    Guid AdminId, Guid RequestId, string Comment) : ICommand;