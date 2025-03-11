using CSharpFunctionalExtensions;
using FilesService.Core.Interfaces;
using FilesService.Core.Models;
using P2Project.SharedKernel.Errors;

namespace P2Project.Volunteers.Application.Files.GetFile
{
    public class GetFileHandler
    {
        private readonly IFileProvider _fileProvider;

        public GetFileHandler(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }
        public async Task<Result<string, Error>> Handle(
            string bucketName, string fileName,
            CancellationToken cancellationToken = default)
        {
            var getFileResult = await _fileProvider.GetFile(
                new FileMetadata(bucketName, fileName),
                cancellationToken);

            return getFileResult;
        }
    }
}
