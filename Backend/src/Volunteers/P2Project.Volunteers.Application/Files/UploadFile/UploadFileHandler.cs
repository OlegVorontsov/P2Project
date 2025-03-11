using CSharpFunctionalExtensions;
using FilesService.Core.Dtos;
using FilesService.Core.Requests.Minio;
using FilesService.Core.ValueObjects;
using P2Project.SharedKernel.Errors;
using IFileProvider = FilesService.Core.Interfaces.IFileProvider;

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
            string bucketName,
            CancellationToken cancellationToken = default)
        {
            var extension = Path.GetExtension(uploadFileDto.FileName);

            var filePathResult = FilePath.Create(
                Guid.NewGuid(), extension);
            if (filePathResult.IsFailure)
                return Errors.General.Failure(filePathResult.Error.Message).ToErrorList();

            var fileInfo = new FileInfoDto(
                filePathResult.Value, bucketName);

            var uploadFileResult = await _fileProvider.UploadFile(
                new UploadFileRequest(
                    uploadFileDto.Stream,
                    fileInfo),
                cancellationToken);

            return uploadFileResult.Value;
        }
    }
}
