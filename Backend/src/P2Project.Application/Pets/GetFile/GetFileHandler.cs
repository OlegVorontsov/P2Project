using CSharpFunctionalExtensions;
using P2Project.Application.FileProvider;
using P2Project.Application.FileProvider.Models;
using P2Project.Domain.Shared;

namespace P2Project.Application.Pets.GetFile
{
    public class GetFileHandler
    {
        private readonly IFileProvider _fileProvider;

        public GetFileHandler(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }
        public async Task<Result<string, Error>> Handle(
            FileMetadata fileMetadata,
            CancellationToken cancellationToken = default)
        {
            var deleteFileResult = await _fileProvider.GetFile(
                fileMetadata,
                cancellationToken);

            return deleteFileResult;
        }
    }
}
