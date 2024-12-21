using CSharpFunctionalExtensions;
using P2Project.Application.FileProvider.Models;
using P2Project.Application.Shared.Dtos;
using P2Project.Application.Shared.Dtos.Files;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.PetManagment.ValueObjects.Files;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.Errors;
using FileInfo = P2Project.Application.FileProvider.Models.FileInfo;
using IFileProvider = P2Project.Application.FileProvider.IFileProvider;

namespace P2Project.Application.Files.UploadFile
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

            var fileInfo = new FileInfo(
                filePathResult.Value, Constants.BUCKET_NAME_FILES);

            var uploadFileResult = await _fileProvider.UploadFile(
                new FileData(
                    uploadFileDto.Stream,
                    fileInfo),
                cancellationToken);

            return uploadFileResult.Value;
        }
    }
}
