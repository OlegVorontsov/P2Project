using CSharpFunctionalExtensions;
using P2Project.Application.FileProvider.Models;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;

namespace P2Project.Application.FileProvider
{
    public interface IFileProvider
    {
        Task<Result<string, Error>> UploadFile(
            FileData fileData,
            CancellationToken cancellationToken = default);

        Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
            IEnumerable<FileData> filesData,
            CancellationToken cancellationToken = default);

        Task<Result<string, Error>> DeleteFile(
            FileMetadata fileMetadata,
            CancellationToken cancellationToken = default);

        Task<Result<string, Error>> GetFile(
            FileMetadata fileMetadata,
            CancellationToken cancellationToken = default);
    }
}
