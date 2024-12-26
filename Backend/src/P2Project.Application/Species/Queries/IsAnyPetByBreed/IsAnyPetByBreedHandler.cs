using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Application.Interfaces.DbContexts;
using P2Project.Application.Interfaces.Queries;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Species.Queries.IsAnyPetByBreed;

public class IsAnyPetByBreedHandler :
    IQueryHandler<bool, IsAnyPetByBreedQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<IsAnyPetByBreedHandler> _logger;

    public IsAnyPetByBreedHandler(
        IReadDbContext readDbContext,
        ILogger<IsAnyPetByBreedHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<Result<bool, ErrorList>> Handle(
        IsAnyPetByBreedQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await _readDbContext.Pets.AnyAsync(p =>
            p.BreedId == query.BreedId, cancellationToken);
        if (result)
            _logger.LogWarning("There're pets with breed id {BreedId}", query.BreedId);
        return result;
    }
}