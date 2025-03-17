using P2Project.Core.Interfaces.Commands;

namespace P2Project.Accounts.Application.Commands.SetAvatar.CompleteSetAvatar;

public record CompleteSetAvatarCommand(
    Guid UserId,
    string FileName,
    string BucketName,
    string UploadId,
    string ETag) : ICommand;
