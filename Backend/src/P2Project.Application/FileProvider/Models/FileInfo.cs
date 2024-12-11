using P2Project.Domain.PetManagment.ValueObjects;

namespace P2Project.Application.FileProvider.Models
{
    public record FileInfo(
        FilePath FilePath,
        string BucketName);
}
