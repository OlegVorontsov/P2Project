using P2Project.Application.Interfaces.Agreements;
using P2Project.Application.Species.Queries.IsAnyPetByBreed;
using P2Project.Application.Species.Queries.IsAnyPetBySpecies;

namespace P2Project.Application.Agreements;

public class PetsAgreement : IPetsAgreement
{
    private readonly IsAnyPetBySpeciesHandler _isAnyPetBySpeciesHandler;
    private readonly IsAnyPetByBreedHandler _isAnyPetByBreedHandler;

    public PetsAgreement(
        IsAnyPetBySpeciesHandler isAnyPetBySpeciesHandler,
        IsAnyPetByBreedHandler isAnyPetByBreedHandler)
    {
        _isAnyPetBySpeciesHandler = isAnyPetBySpeciesHandler;
        _isAnyPetByBreedHandler = isAnyPetByBreedHandler;
    }

    public async Task<bool> IsAnyPetBySpeciesId(
        Guid speciesId, CancellationToken cancellationToken = default)
    {
        var query = new IsAnyPetBySpeciesQuery(speciesId);
        return await _isAnyPetBySpeciesHandler.Handle(query, cancellationToken);
    }

    public async Task<bool> IsAnyPetByBreedId(
        Guid breedId, CancellationToken cancellationToken = default)
    {
        var query = new IsAnyPetByBreedQuery(breedId);
        return await _isAnyPetByBreedHandler.Handle(query, cancellationToken);
    }
}