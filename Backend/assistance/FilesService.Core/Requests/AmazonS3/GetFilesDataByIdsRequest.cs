namespace FilesService.Core.Requests.AmazonS3;

public record GetFilesDataByIdsRequest(IEnumerable<Guid> Ids, string BucketName);