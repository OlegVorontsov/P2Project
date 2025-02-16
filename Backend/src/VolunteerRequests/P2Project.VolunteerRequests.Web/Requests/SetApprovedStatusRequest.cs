using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.SetApprovedStatus;

namespace P2Project.VolunteerRequests.Web.Requests;

public record SetApprovedStatusRequest(string Comment)
{
    public SetApprovedStatusCommand ToCommand(Guid adminId, Guid requestId) =>
        new(adminId, requestId, Comment);
}