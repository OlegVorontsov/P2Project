using CSharpFunctionalExtensions;
using FilesService.Core.ErrorManagment;
using FilesService.Core.Interfaces;
using FilesService.Core.Models;

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
            string bucketName, string fileName,
            CancellationToken cancellationToken = default)
        {
            var deleteFileResult = await _fileProvider
                .DeleteFileByFileMetadata(new FileMetadata(
                    bucketName, fileName), cancellationToken);

            return deleteFileResult;
        }
    }
}
