using P2Project.Core.Interfaces.Commands;

namespace P2Project.Volunteers.Application.Commands.SetAvatar.CompleteSetAvatar;

public record CompleteSetAvatarCommand(
    Guid VolunteerId,
    Guid PetId,
    string FileName,
    string BucketName,
    string UploadId,
    string ETag) : ICommand;