using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.PetManagment.ValueObjects.Files;

namespace P2Project.Application.FileProvider.Models
{
    public record FileInfo(
        FilePath FilePath,
        string BucketName);
}
