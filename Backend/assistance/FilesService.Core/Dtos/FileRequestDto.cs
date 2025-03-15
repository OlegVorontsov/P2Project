using FilesService.Core.ValueObjects;

namespace FilesService.Core.Dtos
{
    public record FileRequestDto(
        Guid FileKey,
        string BucketName,
        string ContentType,
        long Lenght);
}
