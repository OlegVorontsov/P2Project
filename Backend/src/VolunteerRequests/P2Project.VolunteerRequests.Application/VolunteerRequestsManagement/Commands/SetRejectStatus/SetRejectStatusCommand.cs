using P2Project.Core.Interfaces.Commands;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.SetRejectStatus;

public record SetRejectStatusCommand(
    Guid AdminId, Guid RequestId, string Comment, bool IsBanNeed = false) : ICommand;