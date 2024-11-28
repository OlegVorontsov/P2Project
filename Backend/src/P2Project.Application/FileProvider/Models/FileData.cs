using P2Project.Domain.PetManagment.ValueObjects;

namespace P2Project.Application.FileProvider.Models
{
    public record FileData(
        Stream FileStream,
        FilePath FilePath,
        string BucketName);
}
