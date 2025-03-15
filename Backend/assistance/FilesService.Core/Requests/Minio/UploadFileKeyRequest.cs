using FilesService.Core.Dtos;

namespace FilesService.Core.Requests.Minio;

public record UploadFileKeyRequest(
    Stream FileStream,
    FileRequestDto FileRequestDto);