using P2Project.Core.Dtos.Common;
using P2Project.Core.Dtos.Volunteers;
using P2Project.Core.Interfaces.Commands;

namespace P2Project.Volunteers.Application.Commands.Create
{
    public record CreateCommand(
                  VolunteerInfoDto VolunteerInfo,
                  string Gender,
                  string? Description,
                  IEnumerable<PhoneNumberDto>? PhoneNumbers) : ICommand;
}
