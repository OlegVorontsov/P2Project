using P2Project.Application.Shared.Dtos;
using P2Project.Application.Shared.Dtos.Volunteers;
using P2Project.Application.Volunteers.Commands.UpdateMainInfo;

namespace P2Project.API.Controllers.Volunteers.Requests;

public record UpdateMainInfoRequest(
              FullNameDto FullName,
              VolunteerInfoDto VolunteerInfo,
              string Gender,
              string? Description)
{
    public UpdateMainInfoCommand ToCommand(Guid volunteerId) =>
        new(volunteerId, FullName, VolunteerInfo, Gender, Description);
}
