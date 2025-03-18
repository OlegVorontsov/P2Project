namespace P2Project.Volunteers.Web.Requests;

public record CompletePetSetAvatarRequest(
    string FileName,
    string UploadId,
    string ETag);