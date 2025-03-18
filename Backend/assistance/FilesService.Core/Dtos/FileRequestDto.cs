using FilesService.Core.ValueObjects;

namespace FilesService.Core.Dtos
{
    public record FileRequestDto(
        Guid FileKey,
        string BucketName,
        string FileName,
        string ContentType,
        long Lenght);
}
