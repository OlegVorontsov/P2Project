using P2Project.Core.Dtos.Volunteers;
using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands;
using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.Create;

namespace P2Project.VolunteerRequests.Web.Requests;

public record CreateVolunteerRequestRequest(
    Guid UserId,
    FullNameDto FullName,
    VolunteerInfoDto VolunteerInfo,
    string Gender)
{
    public CreateVolunteerRequestCommand ToCommand() =>
        new(UserId,
            FullName,
            VolunteerInfo,
            Gender);
}