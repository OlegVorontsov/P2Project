namespace FilesService.Core.Requests.AmazonS3;

public record UploadPresignedPartUrlRequest(
    string BucketName,
    string UploadId,
    int PartNumber);