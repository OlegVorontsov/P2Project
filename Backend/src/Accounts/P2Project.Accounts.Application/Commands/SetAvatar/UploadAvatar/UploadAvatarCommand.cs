using FilesService.Core.Requests.AmazonS3;
using P2Project.Core.Interfaces.Commands;

namespace P2Project.Accounts.Application.Commands.SetAvatar.UploadAvatar;

public record UploadAvatarCommand(
    byte[] File,
    StartMultipartUploadRequest StartMultipartUploadRequest) : ICommand;