using P2Project.Core.Interfaces.Commands;

namespace P2Project.Volunteers.Application.Commands.ChangePetMainPhoto;

public record ChangePetMainPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    string BucketName,
    string FileName) : ICommand;