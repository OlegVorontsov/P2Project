using FilesService.Core.ValueObjects;

namespace FilesService.Core.Requests.AmazonS3;

public record SaveFilesDataByKeysRequest(
    List<Guid> FilePaths,
    string BucketName,
    string ContentType);