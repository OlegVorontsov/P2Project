using CSharpFunctionalExtensions;
using FilesService.Core.ErrorClasses;
using FilesService.Core.Models;

namespace FilesService.Application.Interfaces;

public interface IFilesRepository
{
    Task Add(FileData fileData, CancellationToken ct);
    Task<Result<FileData>> Get(Guid id, CancellationToken ct);

    Task<Result<IReadOnlyList<FileData>>> GetRange(
        IEnumerable<Guid> ids, CancellationToken ct);

    Task<UnitResult<Error>> Remove(Guid id, CancellationToken ct);

    Task<Result<long, Error>> RemoveRange(
        IEnumerable<Guid> ids, CancellationToken ct);
}