using CSharpFunctionalExtensions;
using P2Project.Core.Errors;
using P2Project.Core.Files.Models;
using P2Project.Core.ValueObjects;
using FileInfo = P2Project.Core.Files.Models.FileInfo;

namespace P2Project.Core.Files
{
    public interface IFileProvider
    {
        Task<Result<string, Error>> UploadFile(
            FileData fileData,
            CancellationToken cancellationToken = default);

        Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
            IEnumerable<FileData> filesData,
            CancellationToken cancellationToken = default);

        Task<Result<string, Error>> DeleteFileByFileMetadata(
            FileMetadata fileMetaData,
            CancellationToken cancellationToken = default);

        Task<UnitResult<Error>> DeleteFileByFileInfo(
            FileInfo fileInfo,
            CancellationToken cancellationToken = default);

        Task<Result<string, Error>> GetFile(
            FileMetadata fileMetadata,
            CancellationToken cancellationToken = default);
    }
}
