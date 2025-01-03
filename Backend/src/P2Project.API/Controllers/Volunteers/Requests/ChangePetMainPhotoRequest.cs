using P2Project.Application.Volunteers.Commands.ChangePetMainPhoto;

namespace P2Project.API.Controllers.Volunteers.Requests;

public record ChangePetMainPhotoRequest(string ObjectName)
{
    public ChangePetMainPhotoCommand ToCommand(Guid volunteerId, Guid petId) =>
        new(volunteerId, petId, ObjectName);
}