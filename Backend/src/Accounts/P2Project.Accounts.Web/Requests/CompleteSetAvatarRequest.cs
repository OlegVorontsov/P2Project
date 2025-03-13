namespace P2Project.Accounts.Web.Requests;

public record CompleteSetAvatarRequest(
    string Key,
    string UploadId,
    string ETag);