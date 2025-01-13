using P2Project.Core.ValueObjects;

namespace P2Project.Core.Files.Models
{
    public record FileInfo(
        FilePath FilePath,
        string BucketName);
}
