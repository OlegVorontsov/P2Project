using P2Project.Application.Volunteers.Commands.ChangePetStatus;

namespace P2Project.API.Controllers.Volunteers.Requests;

public record ChangePetStatusRequest(string Status)
{
    public ChangePetStatusCommand ToCommand(Guid volunteerId, Guid petId) =>
        new(volunteerId, petId, Status.ToLower());
}