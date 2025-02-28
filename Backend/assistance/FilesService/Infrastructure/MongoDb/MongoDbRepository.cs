using CSharpFunctionalExtensions;
using FilesService.Application.Interfaces;
using FilesService.Core.Errors;
using FilesService.Core.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace FilesService.Infrastructure.MongoDb;

public class MongoDbRepository(MongoDbContext dbContext) : IFilesRepository
{
    public async Task Add(FileData fileData, CancellationToken ct)
    {
        await dbContext.Files.InsertOneAsync(fileData, cancellationToken: ct);
    }
    
    public async Task<Result<FileData>> Get(Guid id, CancellationToken ct)
    {
        return await dbContext.Files.AsQueryable()
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken: ct);
    }

    public async Task<Result<IReadOnlyList<FileData>>> GetRange(
        IEnumerable<Guid> ids, CancellationToken ct)
    {
        return await dbContext.Files
            .Find(f => ids.ToList().Contains(f.Id))
            .ToListAsync(ct);
    }

    public async Task<UnitResult<Error>> Remove(Guid id, CancellationToken ct)
    {
        var deleteResult = await dbContext.Files
            .DeleteOneAsync(f => f.Id == id, cancellationToken: ct);
        
        return deleteResult.DeletedCount == 0
            ? Errors.NotFound(id)
            : Result.Success<Error>();
    }
    
    public async Task<Result<long, Error>> RemoveRange(
        IEnumerable<Guid> ids, CancellationToken ct)
    {
        var deleteResult = await dbContext.Files
            .DeleteManyAsync(f => ids.Contains(f.Id), cancellationToken: ct);
        if (deleteResult.DeletedCount == 0)
            return Errors.Failure("There're no items to delete");

        return deleteResult.DeletedCount;
    }
}