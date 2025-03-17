namespace FilesService.Core.Responses.AmazonS3;

public record FilesSaveResponse(List<FileLocationResponse> FileLocationResponses);