using CSharpFunctionalExtensions;
using FilesService.Core.Models;
using P2Project.Core.Files;
using P2Project.SharedKernel;
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
