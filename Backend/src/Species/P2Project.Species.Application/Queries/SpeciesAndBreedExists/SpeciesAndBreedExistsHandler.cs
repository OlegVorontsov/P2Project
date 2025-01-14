using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using P2Project.Core.Interfaces.Queries;
using P2Project.SharedKernel.Errors;

namespace P2Project.Species.Application.Queries.SpeciesAndBreedExists;

public class SpeciesAndBreedExistsHandler :
    IQueryHandler<UnitResult<Error>, SpeciesAndBreedExistsQuery>
{
    private readonly IReadDbContext _readDbContext;

    public SpeciesAndBreedExistsHandler(
        IReadDbContext _readDbContext)
    {
        _readDbContext = _readDbContext;
    }

    public async Task<UnitResult<Error>> Handle(
        SpeciesAndBreedExistsQuery query,
        CancellationToken cancellationToken = default)
    {
        if (await _readDbContext.Species.AnyAsync(
                s => s.Id == query.SpeciesId, cancellationToken) == false)
            return Errors.SpeciesError.NonExistantSpecies(query.SpeciesId);

        if (await _readDbContext.Breeds.AnyAsync
            (b => b.Id == query.BreedId, cancellationToken) == false)
            return Errors.SpeciesError.NonExistantBreed(query.BreedId);

        return UnitResult.Success<Error>();
    }
}