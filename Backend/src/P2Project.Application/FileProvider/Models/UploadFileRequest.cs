
namespace P2Project.Application.FileProvider.Models
{
    public record UploadFileRequest(
        Stream FileStream,
        string FilePath,
        string BucketName);
}
