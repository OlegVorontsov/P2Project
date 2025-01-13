using P2Project.Volunteers.Application.Commands.ChangePetStatus;

namespace P2Project.Volunteers.Web.Requests;

public record ChangePetStatusRequest(string Status)
{
    public ChangePetStatusCommand ToCommand(Guid volunteerId, Guid petId) =>
        new(volunteerId, petId, Status.ToLower());
}