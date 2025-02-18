using P2Project.Core.Interfaces.Commands;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.SetRevisionRequiredStatus;

public record SetRevisionRequiredStatusCommand(
    Guid AdminId, Guid RequestId, string Comment) : ICommand;