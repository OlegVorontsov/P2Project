using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.SetRevisionRequiredStatus;

namespace P2Project.VolunteerRequests.Web.Requests;

public record SetRevisionRequiredStatusRequest(string Comment)
{
    public SetRevisionRequiredStatusCommand ToCommand(Guid adminId, Guid requestId) =>
        new(adminId, requestId, Comment);
}