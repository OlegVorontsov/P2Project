using CSharpFunctionalExtensions;
using P2Project.Application.FileProvider.Models;
using P2Project.Application.Shared.Dtos;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using IFileProvider = P2Project.Application.FileProvider.IFileProvider;

namespace P2Project.Application.Files.CreateFile
{
    public class UploadFileHandler
    {
        private readonly IFileProvider _fileProvider;

        public UploadFileHandler(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }
        public async Task<Result<string, ErrorList>> Handle(
            UploadFileDto uploadFileDto,
            CancellationToken cancellationToken = default)
        {
            var extension = Path.GetExtension(uploadFileDto.FileName);

            var filePathResult = FilePath.Create(
                Guid.NewGuid(), extension);
            if (filePathResult.IsFailure)
                return filePathResult.Error.ToErrorList();

            var uploadFileResult = await _fileProvider.UploadFile(
                new FileData(
                    uploadFileDto.Stream,
                    filePathResult.Value,
                    Constants.BUCKET_NAME_FILES),
                cancellationToken);

            return uploadFileResult.Value;
        }
    }
}
