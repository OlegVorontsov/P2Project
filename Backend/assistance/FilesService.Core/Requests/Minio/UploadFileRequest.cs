using FilesService.Core.Dtos;

namespace FilesService.Core.Requests.Minio;

public record UploadFileRequest(
    Stream FileStream,
    FileInfoDto FileInfoDto);