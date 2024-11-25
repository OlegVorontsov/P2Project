
namespace P2Project.Application.FileProvider.Models
{
    public record FileData(
        IEnumerable<FileContent> Files,
        string BucketName);

    public record FileContent(
        Stream Stream,
        string ObjectName);
}
