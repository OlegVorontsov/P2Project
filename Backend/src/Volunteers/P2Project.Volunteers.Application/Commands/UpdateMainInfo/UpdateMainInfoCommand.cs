using P2Project.Core.Dtos.Volunteers;
using P2Project.Core.Interfaces.Commands;

namespace P2Project.Volunteers.Application.Commands.UpdateMainInfo
{
    public record UpdateMainInfoCommand(
                  Guid VolunteerId,
                  VolunteerInfoDto VolunteerInfo,
                  string Gender,
                  string? Description) : ICommand;
}
