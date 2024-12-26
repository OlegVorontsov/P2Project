using P2Project.Application.Interfaces.Agreements;
using P2Project.Application.Species.Queries;
using P2Project.Application.Species.Queries.IsAnyPetBySpecies;

namespace P2Project.Application.Agreements;

public class PetsAgreement : IPetsAgreement
{
    private readonly IsAnyPetBySpeciesHandler _isAnyPetBySpeciesHandler;

    public PetsAgreement(IsAnyPetBySpeciesHandler isAnyPetBySpeciesHandler)
    {
        _isAnyPetBySpeciesHandler = isAnyPetBySpeciesHandler;
    }

    public async Task<bool> IsAnyPetBySpeciesId(
        Guid speciesId, CancellationToken cancellationToken = default)
    {
        var query = new IsAnyPetBySpeciesQuery(speciesId);
        var result = await _isAnyPetBySpeciesHandler.Handle(query, cancellationToken);
        return result.Value;
    }
}