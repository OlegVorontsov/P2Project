using FilesService.Core.Models;

namespace FilesService.Core.Requests.AmazonS3;

public record CompleteMultipartRequest(
    string BucketName,
    string UploadId,
    List<PartETagInfo> Parts);