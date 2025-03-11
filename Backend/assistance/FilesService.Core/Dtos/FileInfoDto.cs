using FilesService.Core.ValueObjects;

namespace FilesService.Core.Dtos
{
    public record FileInfoDto(
        FilePath FilePath,
        string BucketName);
}
