using P2Project.Volunteers.Application.Commands.ChangePetMainPhoto;

namespace P2Project.Volunteers.Web.Requests;

public record ChangePetMainPhotoRequest(string BucketName, string FileKey, string FileName)
{
    public ChangePetMainPhotoCommand ToCommand(Guid volunteerId, Guid petId) =>
        new(volunteerId, petId, BucketName, FileKey, FileName);
}