using CSharpFunctionalExtensions;
using P2Project.Application.FileProvider.Models;
using P2Project.Domain.Shared;
using IFileProvider = P2Project.Application.FileProvider.IFileProvider;

namespace P2Project.Application.Pets.CreateFile
{
    public class CreateFileHandler
    {
        private readonly IFileProvider _fileProvider;

        public CreateFileHandler(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }
        public async Task<Result<string, Error>> Handle(
            UploadFileRecord uploadFileRecord,
            CancellationToken cancellationToken = default)
        {
            var uploadFileResult = await _fileProvider.UploadFile(
                uploadFileRecord,
                cancellationToken);

            return uploadFileRecord.ObjectName;
        }
    }
}
