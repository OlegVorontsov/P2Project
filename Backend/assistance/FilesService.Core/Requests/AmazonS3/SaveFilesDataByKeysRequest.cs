using FilesService.Core.Dtos;
using FilesService.Core.ValueObjects;

namespace FilesService.Core.Requests.AmazonS3;

public record SaveFilesDataByKeysRequest(
    List<FileRequestDto> FileRequestDtos);