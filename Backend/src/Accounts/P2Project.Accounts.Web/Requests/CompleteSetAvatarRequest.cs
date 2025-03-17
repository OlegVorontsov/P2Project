namespace P2Project.Accounts.Web.Requests;

public record CompleteSetAvatarRequest(
    string FileName,
    string UploadId,
    string ETag);