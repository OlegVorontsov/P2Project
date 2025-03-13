using FilesService.Core.Models;

namespace FilesService.Core.Responses.AmazonS3;

public record UploadFileResponse(
    string Key,
    string UploadId,
    string ETag);