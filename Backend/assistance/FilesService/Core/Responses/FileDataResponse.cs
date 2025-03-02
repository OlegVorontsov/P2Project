namespace FilesService.Core.Responses;

public record FileDataResponse(
    string Key,
    string Url,
    DateTime UploadDate,
    long FileSize,
    string ContentType);