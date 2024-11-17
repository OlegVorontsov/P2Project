using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using P2Project.Application.FileProvider.Models;
using P2Project.Application.Volunteers.CreateVolunteer;
using P2Project.Domain.Shared;
using IFileProvider = P2Project.Application.FileProvider.IFileProvider;

namespace P2Project.Application.Pets.AddPhoto
{
    public class AddPhotoHandler
    {
        private readonly IFileProvider _fileProvider;

        public AddPhotoHandler(IFileProvider fileProvider)
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
