using CSharpFunctionalExtensions;
using P2Project.Application.Interfaces.Agreements;
using P2Project.Application.Species.Queries.SpeciesAndBreedExists;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Agreements;

public class SpeciesAgreement : ISpeciesAgreement
{
    private readonly SpeciesAndBreedExistsHandler _speciesAndBreedExistsHandler;

    public SpeciesAgreement(
        SpeciesAndBreedExistsHandler speciesAndBreedExistsHandler)
    {
        _speciesAndBreedExistsHandler = speciesAndBreedExistsHandler;
    }

    public async Task<UnitResult<Error>> SpeciesAndBreedExists(
        Guid SpeciesId, Guid BreedId,
        CancellationToken cancellationToken = default)
    {
        var query = new SpeciesAndBreedExistsQuery(SpeciesId, BreedId);
        return await _speciesAndBreedExistsHandler.Handle(query, cancellationToken);
    }
}