using CSharpFunctionalExtensions;
using P2Project.Core;
using P2Project.Core.Errors;
using P2Project.Core.Files;
using P2Project.Core.Files.Models;

namespace P2Project.Volunteers.Application.Files.DeleteFile
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
