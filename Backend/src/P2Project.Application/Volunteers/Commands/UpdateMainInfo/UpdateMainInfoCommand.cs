using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Shared.Dtos.Volunteers;

namespace P2Project.Application.Volunteers.Commands.UpdateMainInfo
{
    public record UpdateMainInfoCommand(
                  Guid VolunteerId,
                  FullNameDto FullName,
                  VolunteerInfoDto VolunteerInfo,
                  string Gender,
                  string? Description) : ICommand;
}
