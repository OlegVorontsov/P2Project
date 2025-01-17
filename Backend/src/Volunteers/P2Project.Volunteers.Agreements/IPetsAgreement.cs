namespace P2Project.Volunteers.Agreements;

public interface IPetsAgreement
{
    Task<bool> IsAnyPetBySpeciesId(
        Guid speciesId, CancellationToken cancellationToken = default);
    
    Task<bool> IsAnyPetByBreedId(
        Guid breedId, CancellationToken cancellationToken = default);
}