using P2Project.SharedKernel.ValueObjects;

namespace P2Project.Core.Files.Models
{
    public record FileInfo(
        FilePath FilePath,
        string BucketName);
}
