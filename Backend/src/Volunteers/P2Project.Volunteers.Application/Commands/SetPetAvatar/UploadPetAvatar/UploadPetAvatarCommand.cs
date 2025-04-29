using FilesService.Core.Requests.AmazonS3;
using P2Project.Core.Interfaces.Commands;

namespace P2Project.Volunteers.Application.Commands.SetPetAvatar.UploadPetAvatar;

public record UploadPetAvatarCommand(
    Guid VolunteerId,
    Guid PetId,
    byte[] File,
    StartMultipartUploadRequest StartMultipartUploadRequest) : ICommand;