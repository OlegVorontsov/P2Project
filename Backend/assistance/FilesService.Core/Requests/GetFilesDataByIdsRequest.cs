namespace FilesService.Core.Requests;

public record GetFilesDataByIdsRequest(IEnumerable<Guid> Ids, string BucketName);