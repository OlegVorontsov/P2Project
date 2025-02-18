using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.SetRejectStatus;

namespace P2Project.VolunteerRequests.Web.Requests;

public record SetRejectStatusRequest(string Comment)
{
    public SetRejectStatusCommand ToCommand(Guid adminId, Guid requestId) =>
        new(adminId, requestId, Comment);
}