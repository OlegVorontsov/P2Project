using CSharpFunctionalExtensions;
using P2Project.Core.Files.Models;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.ValueObjects;

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
            FileInfoDto fileInfoDto,
            CancellationToken cancellationToken = default);

        Task<Result<string, Error>> GetFile(
            FileMetadata fileMetadata,
            CancellationToken cancellationToken = default);
    }
}
