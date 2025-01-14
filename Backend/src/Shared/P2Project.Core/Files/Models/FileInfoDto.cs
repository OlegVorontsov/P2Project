using P2Project.SharedKernel.ValueObjects;

namespace P2Project.Core.Files.Models
{
    public record FileInfoDto(
        FilePath FilePath,
        string BucketName);
}
