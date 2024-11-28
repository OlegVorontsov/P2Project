using CSharpFunctionalExtensions;
using P2Project.Application.FileProvider.Models;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using IFileProvider = P2Project.Application.FileProvider.IFileProvider;

namespace P2Project.Application.Pets.CreateFile
{
    public class UploadFileHandler
    {
        private readonly IFileProvider _fileProvider;

        public UploadFileHandler(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }
        public async Task<Result<string, ErrorList>> Handle(
            UploadFileRequest request,
            CancellationToken cancellationToken = default)
        {
            var filePathResult = FilePath.Create(request.FilePath);

            if (filePathResult.IsFailure)
                return filePathResult.Error.ToErrorList();

            var uploadFileResult = await _fileProvider.UploadFile(
                new FileData(
                    request.FileStream,
                    filePathResult.Value,
                    request.BucketName),
                cancellationToken);

            return uploadFileResult.Value;
        }
    }
}
