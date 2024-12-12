using CSharpFunctionalExtensions;
using P2Project.Application.FileProvider;
using P2Project.Application.FileProvider.Models;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using FileInfo = P2Project.Application.FileProvider.Models.FileInfo;

namespace P2Project.Application.Files.DeleteFile
{
    public class DeleteFileHandler
    {
        private readonly IFileProvider _fileProvider;

        public DeleteFileHandler(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }
        public async Task<Result<string, Error>> Handle(
            string objectName,
            CancellationToken cancellationToken = default)
        {
            var deleteFileResult = await _fileProvider
                .DeleteFileByFileMetadata(new FileMetadata(
                Constants.BUCKET_NAME_FILES, objectName), cancellationToken);

            return deleteFileResult;
        }
    }
}
