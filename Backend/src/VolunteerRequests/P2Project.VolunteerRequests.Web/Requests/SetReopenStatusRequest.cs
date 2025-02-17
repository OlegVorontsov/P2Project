using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.SetReopenStatus;

namespace P2Project.VolunteerRequests.Web.Requests;

public record SetReopenStatusRequest(string Comment)
{
    public SetReopenStatusCommand ToCommand(Guid userId, Guid requestId) =>
        new(userId, requestId, Comment);
}