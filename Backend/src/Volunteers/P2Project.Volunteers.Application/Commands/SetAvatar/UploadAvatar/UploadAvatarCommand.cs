using FilesService.Core.Requests.AmazonS3;
using P2Project.Core.Interfaces.Commands;

namespace P2Project.Volunteers.Application.Commands.SetAvatar.UploadAvatar;

public record UploadAvatarCommand(
    Guid VolunteerId,
    Guid PetId,
    byte[] File,
    StartMultipartUploadRequest StartMultipartUploadRequest) : ICommand;