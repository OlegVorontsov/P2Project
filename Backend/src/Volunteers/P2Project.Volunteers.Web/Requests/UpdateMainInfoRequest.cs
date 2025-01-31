using P2Project.Core.Dtos.Volunteers;
using P2Project.Volunteers.Application.Commands.UpdateMainInfo;

namespace P2Project.Volunteers.Web.Requests;

public record UpdateMainInfoRequest(
              VolunteerInfoDto VolunteerInfo,
              string Gender,
              string? Description)
{
    public UpdateMainInfoCommand ToCommand(Guid volunteerId) =>
        new(volunteerId, VolunteerInfo, Gender, Description);
}
