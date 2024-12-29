using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using P2Project.Application.Interfaces.DbContexts.Species;
using P2Project.Application.Interfaces.Queries;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Species.Queries.SpeciesAndBreedExists;

public class SpeciesAndBreedExistsHandler :
    IQueryHandler<UnitResult<Error>, SpeciesAndBreedExistsQuery>
{
    private readonly ISpeciesReadDbContext _speciesReadDbContext;

    public SpeciesAndBreedExistsHandler(
        ISpeciesReadDbContext speciesReadDbContext)
    {
        _speciesReadDbContext = speciesReadDbContext;
    }

    public async Task<UnitResult<Error>> Handle(
        SpeciesAndBreedExistsQuery query,
        CancellationToken cancellationToken = default)
    {
        if (await _speciesReadDbContext.Species.AnyAsync(
                s => s.Id == query.SpeciesId, cancellationToken) == false)
            return Errors.Species.NonExistantSpecies(query.SpeciesId);

        if (await _speciesReadDbContext.Breeds.AnyAsync
            (b => b.Id == query.BreedId, cancellationToken) == false)
            return Errors.Species.NonExistantBreed(query.BreedId);

        return UnitResult.Success<Error>();
    }
}