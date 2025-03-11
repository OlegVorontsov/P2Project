namespace FilesService.Core.Requests.AmazonS3;

public record UploadPresignedUrlRequest(
    string BucketName,
    string FileName,
    string ContentType,
    long Size);