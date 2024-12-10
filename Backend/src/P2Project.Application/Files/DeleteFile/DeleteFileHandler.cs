using CSharpFunctionalExtensions;
using P2Project.Application.FileProvider;
using P2Project.Application.FileProvider.Models;
using P2Project.Domain.Shared;

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
            FileMetadata fileMetadata,
            CancellationToken cancellationToken = default)
        {
            var deleteFileResult = await _fileProvider.DeleteFile(
                fileMetadata,
                cancellationToken);

            return deleteFileResult.IsSuccess.ToString();
        }
    }
}
