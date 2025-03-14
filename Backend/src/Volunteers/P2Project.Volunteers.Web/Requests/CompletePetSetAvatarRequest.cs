namespace P2Project.Volunteers.Web.Requests;

public record CompletePetSetAvatarRequest(
    string Key,
    string UploadId,
    string ETag);