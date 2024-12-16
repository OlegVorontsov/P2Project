using CSharpFunctionalExtensions;
using P2Project.Application.FileProvider;
using P2Project.Application.FileProvider.Models;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Files.GetFile
{
    public class GetFileHandler
    {
        private readonly IFileProvider _fileProvider;

        public GetFileHandler(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }
        public async Task<Result<string, Error>> Handle(
            string id,
            CancellationToken cancellationToken = default)
        {
            var getFileResult = await _fileProvider.GetFile(
                new FileMetadata(Constants.BUCKET_NAME_FILES, id),
                cancellationToken);

            return getFileResult;
        }
    }
}
