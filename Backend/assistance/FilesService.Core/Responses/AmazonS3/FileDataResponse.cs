namespace FilesService.Core.Responses.AmazonS3;

public record FileDataResponse(
    string Key,
    string Url,
    DateTime UploadDate,
    long FileSize,
    string ContentType);