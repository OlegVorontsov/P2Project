using FilesService.Core.Models;

namespace FilesService.Core.Requests;

public record CompleteMultipartRequest(
    string BucketName,
    string UploadId,
    List<PartETagInfo> Parts);