using CSharpFunctionalExtensions;
using P2Project.Core;
using P2Project.Core.Dtos.Files;
using P2Project.Core.Files.Models;
using P2Project.SharedKernel;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.ValueObjects;
using FileInfo = P2Project.Core.Files.Models.FileInfo;
using IFileProvider = P2Project.Core.Files.IFileProvider;

namespace P2Project.Volunteers.Application.Files.UploadFile
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
