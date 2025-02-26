namespace FilesService.Core.Requests;

public record UploadPresignedPartUrlRequest(
    string BucketName,
    string UploadId,
    int PartNumber);