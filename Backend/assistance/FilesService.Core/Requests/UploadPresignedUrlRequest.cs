namespace FilesService.Core.Requests;

public record UploadPresignedUrlRequest(
    string BucketName,
    string FileName,
    string ContentType,
    long Size);