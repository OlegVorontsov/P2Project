using CSharpFunctionalExtensions;
using P2Project.Core.Errors;
using P2Project.Species.Agreements;
using P2Project.Species.Application.Queries.SpeciesAndBreedExists;

namespace P2Project.Species.Web;

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