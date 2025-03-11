namespace FilesService.Core.Responses.AmazonS3;

public record UploadPartFileResponse(string Key, string UploadId);