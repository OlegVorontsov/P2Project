using CSharpFunctionalExtensions;
using P2Project.Application.FileProvider.Models;
using P2Project.Domain.Shared;

namespace P2Project.Application.FileProvider
{
    public interface IFileProvider
    {
        Task<Result<string, Error>> UploadFile(
            UploadFileRecord uploadFileRecord,
            CancellationToken cancellationToken = default);
    }
}
