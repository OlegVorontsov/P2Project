using P2Project.Core.Interfaces.Commands;

namespace P2Project.Volunteers.Application.Commands.SetPetAvatar.CompleteSetPetAvatar;

public record CompleteSetPetAvatarCommand(
    Guid VolunteerId,
    Guid PetId,
    string FileName,
    string BucketName,
    string UploadId,
    string ETag) : ICommand;