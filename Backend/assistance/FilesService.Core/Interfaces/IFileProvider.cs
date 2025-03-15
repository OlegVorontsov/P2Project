using CSharpFunctionalExtensions;
using FilesService.Core.Dtos;
using FilesService.Core.ErrorManagment;
using FilesService.Core.Models;
using FilesService.Core.Requests.Minio;
using FilesService.Core.ValueObjects;

namespace FilesService.Core.Interfaces
{
    public interface IFileProvider
    {
        Task<Result<string, Error>> UploadFile(
            UploadFileRequest uploadFileRequest,
            CancellationToken cancellationToken = default);

        Task<Result<IReadOnlyList<Guid>, Error>> UploadFiles(
            IEnumerable<UploadFileKeyRequest> uploadFileRequest,
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
