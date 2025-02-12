using P2Project.Core.Dtos.Volunteers;
using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands;

namespace P2Project.VolunteerRequests.Web.Requests;

public record CreateVolunteerRequestRequest(
    Guid UserId,
    FullNameDto FullName,
    VolunteerInfoDto VolunteerInfo)
{
    public CreateVolunteerRequestCommand ToCommand() =>
        new(UserId,
            FullName,
            VolunteerInfo);
}