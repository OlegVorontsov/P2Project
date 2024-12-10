
using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.FileProvider.Models
{
    public record UploadFileRequest(
        UploadFileDto FileDto,
        string BucketName);
}
