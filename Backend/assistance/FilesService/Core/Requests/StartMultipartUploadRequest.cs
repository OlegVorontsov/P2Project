namespace FilesService.Core.Requests;

public record StartMultipartUploadRequest(
    string BucketName,
    string FileName,
    string ContentType,
    long Size);