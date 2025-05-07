using P2Project.Core.Dtos.Volunteers;
using P2Project.Core.Interfaces.Commands;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.Create;

public record CreateVolunteerRequestCommand(
    Guid UserId,
    FullNameDto FullName,
    VolunteerInfoDto VolunteerInfo,
    string Gender) : ICommand;