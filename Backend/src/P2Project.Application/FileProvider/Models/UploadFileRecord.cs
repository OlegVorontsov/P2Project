
namespace P2Project.Application.FileProvider.Models
{
    public record UploadFileRecord(
        Stream Stream,
        string BucketName,
        string ObjectName);
}
