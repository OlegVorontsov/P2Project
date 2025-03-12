namespace FilesService.Core.Requests.AmazonS3;

public record StartMultipartUploadRequest(
    string BucketName,
    string FileName,
    string ContentType,
    long Size);