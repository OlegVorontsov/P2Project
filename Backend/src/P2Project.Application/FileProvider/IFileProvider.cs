using CSharpFunctionalExtensions;
using P2Project.Application.FileProvider.Models;
using P2Project.Domain.Shared;

namespace P2Project.Application.FileProvider
{
    public interface IFileProvider
    {
        public async Task<Result<string, Error>> UploadFiles(
            FileData fileData,
            CancellationToken cancellationToken = default);

        Task<Result<string, Error>> DeleteFile(
            FileMetadata fileMetadata,
            CancellationToken cancellationToken = default);

        Task<Result<string, Error>> GetFileURL(
            FileMetadata fileMetadata,
            CancellationToken cancellationToken = default);
    }
}
